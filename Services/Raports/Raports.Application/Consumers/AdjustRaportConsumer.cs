namespace Raports.Application.Consumers;

internal class AdjustRaportConsumer(ILogger<AdjustRaportConsumer> logger,
                                    IPublishEndpoint publish,
                                    RaportsDBContext database) : IConsumer<AdjustRaport>
{
    public async Task Consume(ConsumeContext<AdjustRaport> context)
    {
        logger.LogInformation("AdjustRaportConsumer: adjusting RaportID={RaportId}", context.Message.Raport.ID);

        var ct = context.CancellationToken;

        // helper to publish RaportFailed
        async Task PublishRaportFailedAsync(DefaultRaportDTO raportDto, int raportId, string description, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogWarning("AdjustRaportConsumer: publishing RaportFailed for Raport {RaportId}: {Description}", raportId, description);
                var message = new RaportFailed()
                {
                    FailedDate = DateTime.UtcNow,
                    Description = description,
                    Raport = raportDto
                };

                await publish.Publish(message, cancellationToken);
                logger.LogInformation("AdjustRaportConsumer: RaportFailed published for Raport {RaportId}", raportId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "AdjustRaportConsumer: error while publishing RaportFailed for Raport {RaportId}", raportId);
            }
        }

        try
        {
            var dbRaport = await database.Raports
                .Include(r => r.Period)
                // include measurement and location on measurement groups
                .Include(r => r.MeasurementGroups).ThenInclude(mg => mg.Measurement)
                .Include(r => r.MeasurementGroups).ThenInclude(mg => mg.LocationGroups).ThenInclude(lg => lg.Location)
                .Include(r => r.MeasurementGroups).ThenInclude(mg => mg.LocationGroups).ThenInclude(lg => lg.SampleGroups)
                .FirstOrDefaultAsync(r => r.ID == context.Message.Raport.ID, ct);

            if (dbRaport is null)
            {
                logger.LogWarning("AdjustRaportConsumer: Raport {RaportId} not found", context.Message.Raport.ID);
                return;
            }

            var period = dbRaport.Period ?? await database.Periods.FirstOrDefaultAsync(p => p.ID == dbRaport.PeriodID, ct);
            var timeframe = period?.TimeFrame ?? TimeSpan.FromHours(1);
            if (timeframe <= TimeSpan.Zero) timeframe = TimeSpan.FromHours(1);

            var start = dbRaport.StartDate;
            var end = dbRaport.EndDate;

            // canonical timepoints
            var canonicalTimepoints = new List<DateTime>();
            for (var tp = start; tp <= end; tp = tp.Add(timeframe)) canonicalTimepoints.Add(tp);
            if (canonicalTimepoints.Count == 0 || canonicalTimepoints.Last() < end) canonicalTimepoints.Add(end);

            var toAdd = new List<SampleGroup>();

            foreach (var measurementGroup in dbRaport.MeasurementGroups)
            {
                var measurementName = measurementGroup.Measurement?.Name ?? "<unknown>";

                foreach (var locationGroup in measurementGroup.LocationGroups)
                {
                    var locationName = locationGroup.Location?.Name ?? "<unknown>";

                    // existing samples for this location group
                    var existing = locationGroup.SampleGroups
                        .OrderBy(s => s.Date)
                        .ToList();

                    // quick lookup of existing dates
                    var existingDates = new HashSet<DateTime>(existing.Select(s => s.Date));

                    if (existing.Count == 0)
                    {
                        logger.LogWarning("AdjustRaportConsumer: no samples for Measurement '{Measurement}' at Location '{Location}', skipping interpolation", measurementName, locationName);
                        continue;
                    }

                    // for each canonical timepoint, if missing - compute value
                    foreach (var tp in canonicalTimepoints)
                    {
                        if (existingDates.Contains(tp)) continue; // already present

                        // find neighbours
                        var prev = existing.LastOrDefault(s => s.Date < tp);
                        var next = existing.FirstOrDefault(s => s.Date > tp);

                        double? value = null;

                        if (prev is not null && next is not null)
                        {
                            var total = (next.Date - prev.Date).TotalSeconds;
                            if (total <= 0)
                            {
                                value = prev.Value; // degenerate
                            }
                            else
                            {
                                var offset = (tp - prev.Date).TotalSeconds;
                                var frac = offset / total;
                                value = prev.Value + (next.Value - prev.Value) * frac;
                            }
                        }
                        else if (prev is not null)
                        {
                            value = prev.Value; // carry forward
                        }
                        else if (next is not null)
                        {
                            value = next.Value; // carry backward
                        }

                        if (value.HasValue)
                        {
                            var sample = new SampleGroup
                            {
                                Date = tp,
                                Value = value.Value,
                                LocationGroupID = locationGroup.ID
                            };

                            toAdd.Add(sample);

                            // Also add to in-memory lists so subsequent points can use them
                            existing.Add(new SampleGroup { Date = tp, Value = value.Value, LocationGroupID = locationGroup.ID });
                            existing = existing.OrderBy(s => s.Date).ToList();
                            existingDates.Add(tp);
                        }
                        else
                        {
                            logger.LogWarning("AdjustRaportConsumer: unable to compute value for Measurement '{Measurement}' at Location '{Location}' for time {Time}", measurementName, locationName, tp);
                        }
                    }
                }
            }

            if (toAdd.Count > 0)
            {
                try
                {
                    await database.SampleGroups.AddRangeAsync(toAdd, ct);
                    await database.SaveChangesAsync(ct);

                    logger.LogInformation("AdjustRaportConsumer: added {Count} interpolated samples for Raport {RaportId}", toAdd.Count, dbRaport.ID);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "AdjustRaportConsumer: error while saving interpolated samples for Raport {RaportId}", dbRaport.ID);
                    // publish failure
                    await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, "Error while saving interpolated samples: " + ex.Message, ct);
                    return;
                }
            }
            else
            {
                logger.LogInformation("AdjustRaportConsumer: no interpolated samples needed for Raport {RaportId}", dbRaport.ID);
            }

            var message = new GenerateSummary()
            {
                Raport = context.Message.Raport
            };

            await publish.Publish(message, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AdjustRaportConsumer: unexpected error while adjusting Raport {RaportId}", context.Message.Raport.ID);
            await PublishRaportFailedAsync(context.Message.Raport, context.Message.Raport.ID, "Unexpected error: " + ex.Message, ct);
            return;
        }
    }
}
