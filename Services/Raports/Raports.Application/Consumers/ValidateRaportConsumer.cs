namespace Raports.Application.Consumers;

internal class ValidateRaportConsumer(ILogger<ValidateRaportConsumer> logger,
                                       IPublishEndpoint publish,
                                       RaportsDBContext database) : IConsumer<ValidateRaport>
{
    public async Task Consume(ConsumeContext<ValidateRaport> context)
    {
        logger.LogInformation($"ValidateRaportConsumer: validating RaportID={context.Message.Raport.ID}");

        var dbRaport = await database.Raports
            .Include(x => x.RequestedMeasurements)
            .Include(x => x.RequestedLocations)
            .Include(x => x.MeasurementGroups)
                .ThenInclude(y => y.LocationGroups)
                    .ThenInclude(z => z.SampleGroups)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == context.Message.Raport.ID);

        var ct = context.CancellationToken;

        if (dbRaport is null)
        {
            var desc = $"Raport {context.Message.Raport.ID} not found in DB";

            logger.LogWarning(desc);
            await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, desc, ct);

            return;
        }

        // quick access
        var requestedLocations = context.Message.Raport.RequestedLocations;
        var requestedMeasurements = context.Message.Raport.RequestedMeasurements;

        // 1) Check whether report has all of requested measurements and locations
        foreach (var reqMes in requestedMeasurements)
        {
            var mesGroup = dbRaport.MeasurementGroups.FirstOrDefault(x => x.Measurement != null && x.Measurement.Name == reqMes.Name);
            if (mesGroup is null)
            {
                var desc = $"Requested measurement '{reqMes.Name}' not present in Raport ID {dbRaport.ID}";
                await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, desc, ct);
                return;
            }

            foreach (var reqLoc in requestedLocations)
            {
                var hasLocationGroup = mesGroup.LocationGroups.Any(x => x.Location != null && x.Location.Name == reqLoc.Name);
                if (!hasLocationGroup)
                {
                    var desc = $"Measurement '{reqMes.Name}' missing location '{reqLoc.Name}' in Raport ID {dbRaport.ID}";
                    await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, desc, ct);
                    return;
                }
            }
        }

        // 2) Now check missing samples per measurement/location against Period.MaxAcceptableMissingTimeFrame
        var period = dbRaport.Period ?? await database.Periods.FirstOrDefaultAsync(p => p.ID == dbRaport.PeriodID);
        var timeframe = period?.TimeFrame ?? TimeSpan.FromHours(1);
        if (timeframe <= TimeSpan.Zero) timeframe = TimeSpan.FromHours(1);

        var maxMissing = period?.MaxAcceptableMissingTimeFrame ?? 0;

        var start = dbRaport.StartDate;
        var end = dbRaport.EndDate;

        // canonical timepoints
        var canonicalTimepoints = new List<DateTime>();
        for (var tp = start; tp <= end; tp = tp.Add(timeframe)) canonicalTimepoints.Add(tp);
        if (canonicalTimepoints.Count == 0 || canonicalTimepoints.Last() < end) canonicalTimepoints.Add(end);

        // For each measurement group and its location groups, count missing sample points
        foreach (var measurementGroup in dbRaport.MeasurementGroups)
        {
            var measurementName = measurementGroup.Measurement?.Name ?? "<unknown>";

            foreach (var locationGroup in measurementGroup.LocationGroups)
            {
                var locationName = locationGroup.Location?.Name ?? "<unknown>";

                // Build set of sample dates present (normalize to canonical points by exact match)
                var sampleDates = new HashSet<DateTime>(locationGroup.SampleGroups.Select(s => s.Date));

                if (sampleDates.Count == 0)
                {
                    var desc = $"Measurement '{measurementName}' was not captured at location '{locationName}'";
                    await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, desc, ct);
                    return;
                }

                int missingCount = canonicalTimepoints.Count(tp => !sampleDates.Contains(tp));

                if (missingCount > maxMissing)
                {
                    var desc = $"Measurement '{measurementName}' at location '{locationName}' missing {missingCount} samples (allowed {maxMissing}) in Raport ID {dbRaport.ID}";
                    await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, desc, ct);
                    return;
                }
            }
        }

        try
        {
            await publish.Publish(new AdjustRaport { Raport = context.Message.Raport }, ct);
        }
        catch (Exception ex)
        {
            string desc = "Error while publishing RaportPending for Raport {dbRaport.ID}";
            logger.LogError(ex, desc);
            await PublishRaportFailedAsync(context.Message.Raport, dbRaport.ID, desc, ct);
        }
    }

    private async Task PublishRaportFailedAsync(DefaultRaportDTO raportDto, int raportId, string description, CancellationToken ct)
    {
        try
        {
            logger.LogWarning("Validation failed for Raport {RaportId}: {Description}", raportId, description);

            var message = new RaportFailed()
            {
                FailedDate = DateTime.UtcNow,
                Description = description,
                Raport = raportDto
            };

            await publish.Publish(message, ct);

            logger.LogInformation("RaportFailed published for Raport {RaportId}", raportId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while publishing RaportFailed for Raport {RaportId}", raportId);
            throw;
        }
    }
}
