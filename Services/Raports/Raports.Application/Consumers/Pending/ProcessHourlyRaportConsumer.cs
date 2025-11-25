using CommonServiceLibrary.GRPC.Types.Measurements;

namespace Raports.Application.Consumers.Pending;

internal class ProcessHourlyRaportConsumer(ILogger<ProcessHourlyRaportConsumer> logger,
                                           IPublishEndpoint publish,
                                           RaportsDBContext database,
                                           IMemoryCache cashe,
                                           MeasurementService.MeasurementServiceClient measurementsGRPC,
                                           DevicesService.DevicesServiceClient deviceGRPC) : IConsumer<RaportPending>
{
    public async Task Consume(ConsumeContext<RaportPending> context)
    {
        logger.LogInformation($"Processing Hourly raport");

        var cache = cashe; // keep original parameter name but use a clearer local variable
        var ct = context.CancellationToken;

        // Fetch and cache LocationGRPC objects from Devices service for requested locations
        var cashedLocations = new List<LocationGRPC>();
        foreach (var requestedLocation in context.Message.Raport.RequestedLocations)
        {
            var key = $"{nameof(LocationGRPC)}:{requestedLocation.Hash}";

            var cashedLocation = cache.Get<LocationGRPC>(key);
            if (cashedLocation is null)
            {
                try
                {
                    var response = await deviceGRPC.GetLocationByNameAsync(new LocationByNameRequest() { Name = requestedLocation.Name }).ResponseAsync.WaitAsync(ct);
                    var castedResponse = response.Adapt<LocationGRPC>();

                    cache.Set(key, castedResponse);
                    cashedLocation = castedResponse;
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Location lookup canceled for {Location}", requestedLocation.Name);
                    throw;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Failed to fetch location '{Location}' from Devices service; skipping location", requestedLocation.Name);
                    continue;
                }
            }

            cashedLocations.Add(cashedLocation);
        }

        if (cashedLocations.Count == 0)
        {
            logger.LogInformation("No locations to query measurements for; aborting.");
            return;
        }

        // Get Measurements from Measurements service via gRPC
        var grpcMeasurements = new List<MeasurementGRPC>();
        var request = new MeasurementsQueryRequest()
        {
            SortOrder = "asc",
            StartDate = context.Message.Raport.StartDate.ToString("o"),
            EndDate = context.Message.Raport.EndDate.ToString("o"),
        };

        foreach (var loc in cashedLocations)
        {
            request.LocationIds.Add(loc.Hash.ToString() ?? string.Empty);
        }

        try
        {
            using (var call = measurementsGRPC.MeasurementsQuery(request))
            {
                while (await call.ResponseStream.MoveNext(ct))
                {
                    var current = call.ResponseStream.Current;

                    if (!Guid.TryParse(current.Id, out var id))
                    {
                        logger.LogWarning("Skipping measurement with invalid Id: {Id}", current.Id);
                        continue;
                    }

                    if (!Guid.TryParse(current.DeviceNumber, out var deviceNumber))
                    {
                        logger.LogWarning("Skipping measurement with invalid DeviceNumber: {DeviceNumber}", current.DeviceNumber);
                        continue;
                    }

                    if (!DateTime.TryParse(current.MeasurementCaptureDate, out var measurementCaptureDate))
                    {
                        logger.LogWarning("Skipping measurement with invalid MeasurementCaptureDate: {Date}", current.MeasurementCaptureDate);
                        continue;
                    }

                    if (!Guid.TryParse(current.LocationHash, out var locationHash))
                    {
                        logger.LogWarning("Skipping measurement with invalid LocationHash: {LocationHash}", current.LocationHash);
                        continue;
                    }

                    var mes = new MeasurementGRPC(
                        id,
                        deviceNumber,
                        measurementCaptureDate,
                        locationHash,

                        current.Temperature == double.MinValue ? null : current.Temperature,
                        current.Humidity == double.MinValue ? null : current.Humidity,
                        current.Co2 == double.MinValue ? null : current.Co2,
                        current.Voc == double.MinValue ? null : current.Voc,
                        current.ParticulateMatter1 == double.MinValue ? null : current.ParticulateMatter1,
                        current.ParticulateMatter2V5 == double.MinValue ? null : current.ParticulateMatter2V5,
                        current.ParticulateMatter10 == double.MinValue ? null : current.ParticulateMatter10,
                        current.Formaldehyde == double.MinValue ? null : current.Formaldehyde,
                        current.Co == double.MinValue ? null : current.Co,
                        current.O3 == double.MinValue ? null : current.O3,
                        current.Ammonia == double.MinValue ? null : current.Ammonia,
                        current.Airflow == double.MinValue ? null : current.Airflow,
                        current.AirIonizationLevel == double.MinValue ? null : current.AirIonizationLevel,
                        current.O2 == double.MinValue ? null : current.O2,
                        current.Radon == double.MinValue ? null : current.Radon,
                        current.Illuminance == double.MinValue ? null : current.Illuminance,
                        current.SoundLevel == double.MinValue ? null : current.SoundLevel
                    );

                    grpcMeasurements.Add(mes);
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Measurement streaming canceled");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while querying measurements");
        }

        // sort oldest to newest and group by location
        var measurementsByLocation = grpcMeasurements
            .OrderBy(m => m.MeasurementCaptureDate)
            .GroupBy(m => m.LocationHash)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Determine timeframe for buckets from the Raport Period
        TimeSpan timeframe = TimeSpan.Zero;
        try
        {
            timeframe = context.Message.Raport.Period?.TimeFrame ?? TimeSpan.Zero;
        }
        catch
        {
            timeframe = TimeSpan.Zero;
        }

        if (timeframe <= TimeSpan.Zero)
        {
            logger.LogWarning("Period timeframe is not set or invalid. Falling back to 1 hour.");
            timeframe = TimeSpan.FromHours(1);
        }

        var start = context.Message.Raport.StartDate;
        var end = context.Message.Raport.EndDate;

        // For each location, create centered windows for each timepoint from start..end (inclusive of end)
        var groupedByLocationAndTime = new Dictionary<Guid, List<(DateTime TimePoint, DateTime WindowStart, DateTime WindowEnd, List<MeasurementGRPC> Measurements)>>();

        foreach (var loc in measurementsByLocation)
        {
            var buckets = new List<(DateTime TimePoint, DateTime WindowStart, DateTime WindowEnd, List<MeasurementGRPC>)>();

            // build timepoints: start, start+timeframe, ..., up to <= end. Always include end as last point.
            var timepoints = new List<DateTime>();
            for (var tp = start; tp <= end; tp = tp.Add(timeframe))
            {
                timepoints.Add(tp);
            }
            if (timepoints.Count == 0 || timepoints.Last() < end)
            {
                timepoints.Add(end);
            }

            var half = TimeSpan.FromTicks((long)(timeframe.Ticks / 2.0));

            foreach (var tp in timepoints)
            {
                var windowStart = tp - half;
                var windowEnd = tp + half;

                // clamp to report range
                if (windowStart < start) windowStart = start;
                if (windowEnd > end) windowEnd = end;

                // include measurements: >= windowStart and < windowEnd, but include measurements equal to end for last bucket
                List<MeasurementGRPC> items;
                if (windowEnd == end)
                {
                    items = loc.Value.Where(m => m.MeasurementCaptureDate >= windowStart && m.MeasurementCaptureDate <= windowEnd).ToList();
                }
                else
                {
                    items = loc.Value.Where(m => m.MeasurementCaptureDate >= windowStart && m.MeasurementCaptureDate < windowEnd).ToList();
                }

                buckets.Add((tp, windowStart, windowEnd, items));
            }

            groupedByLocationAndTime[loc.Key] = buckets;
        }

        // Aggregate averages per bucket excluding nulls

        var aggregatedByLocation = new Dictionary<Guid, List<AggregatedBucket>>();

        foreach (var kv in groupedByLocationAndTime)
        {
            var list = new List<AggregatedBucket>();

            foreach (var bucket in kv.Value)
            {
                var items = bucket.Measurements;

                double? Avg(IEnumerable<double?> seq)
                {
                    var vals = seq.Where(x => x.HasValue).Select(x => x!.Value).ToList();
                    return vals.Count == 0 ? null : (double?)vals.Average();
                }

                var agg = new AggregatedBucket(
                    bucket.TimePoint,
                    bucket.WindowStart,
                    bucket.WindowEnd,
                    Avg(items.Select(x => x.Temperature)),
                    Avg(items.Select(x => x.Humidity)),
                    Avg(items.Select(x => x.CarbonDioxide)),
                    Avg(items.Select(x => x.VolatileOrganicCompounds)),
                    Avg(items.Select(x => x.ParticulateMatter1)),
                    Avg(items.Select(x => x.ParticulateMatter2v5)),
                    Avg(items.Select(x => x.ParticulateMatter10)),
                    Avg(items.Select(x => x.Formaldehyde)),
                    Avg(items.Select(x => x.CarbonMonoxide)),
                    Avg(items.Select(x => x.Ozone)),
                    Avg(items.Select(x => x.Ammonia)),
                    Avg(items.Select(x => x.Airflow)),
                    Avg(items.Select(x => x.AirIonizationLevel)),
                    Avg(items.Select(x => x.Oxygen)),
                    Avg(items.Select(x => x.Radon)),
                    Avg(items.Select(x => x.Illuminance)),
                    Avg(items.Select(x => x.SoundLevel)),
                    items.Count);

                list.Add(agg);
            }

            aggregatedByLocation[kv.Key] = list;
        }

        // Persist aggregated data into Raports DB
        // create raport
        var periodId = context.Message.Raport.Period?.ID ?? 0;
        if (periodId == 0)
        {
            var period = await database.Periods.FirstOrDefaultAsync(p => p.TimeFrame == timeframe);
            periodId = period?.ID ?? 1;
        }

        var dbLocations = await database.Locations.ToListAsync();
        var status = await database.Statuses.FirstOrDefaultAsync(s => s.Name == "Completed") ?? await database.Statuses.FirstOrDefaultAsync();

        var raport = await database.Raports.FirstOrDefaultAsync(x => x.ID == context.Message.Raport.ID);
        if (raport is null)
        {
            // ?
            throw new InvalidOperationException();
        }

        // prepare mapping and lookups
        var measurementsLookup = await database.Measurements.ToListAsync();

        var measurementMap = new Dictionary<string, string>()
        {
            { "Temperature", "Air Temperature" },
            { "Humidity", "Relative Humidity" },
            { "CarbonDioxide", "Carbon Dioxide" },
            { "VolatileOrganicCompounds", "Volatile Organic Compounds" },
            { "ParticulateMatter1", "Particulate Matter 1um" },
            { "ParticulateMatter2v5", "Particulate Matter 2.5um" },
            { "ParticulateMatter10", "Particulate Matter 10um" },
            { "Formaldehyde", "Formaldehyde" },
            { "CarbonMonoxide", "Carbon Monoxide" },
            { "Ozone", "Ozone" },
            { "Ammonia", "Ammonia" },
            { "Airflow", "Air Flow Rate" },
            { "AirIonizationLevel", "Air Ionization Level" },
            { "Oxygen", "Oxygen Concentration" },
            { "Radon", "Radon Concentration" },
            { "Illuminance", "Illuminance level" },
            { "SoundLevel", "Sound Pressure Level" }
        };

        foreach (var mapEntry in measurementMap)
        {
            var key = mapEntry.Key; // property name on AggregatedBucket
            var measurementName = mapEntry.Value;

            var measurementEntity = measurementsLookup.FirstOrDefault(m => m.Name == measurementName);
            if (measurementEntity is null)
            {
                logger.LogWarning($"Measurement with name '{measurementName}' was not recognized!");
                continue;
            }

            var isMeasurementTypeRequested = context.Message.Raport.RequestedMeasurements.Any(x => x.Name == measurementName);
            if (isMeasurementTypeRequested == false)
            {
                logger.LogWarning($"Measurement '{measurementEntity.Name}' was not requested for this raport, skipping.");
                continue;
            }
            else
            {
                logger.LogWarning($"Creating Measurement Group for '{measurementEntity.Name}'");
            }

            var measurementGroup = new MeasurementGroup
            {
                MeasurementID = measurementEntity.ID,
                RaportID = raport.ID,
                Summary = string.Empty
            };

            await database.MeasurementGroups.AddAsync(measurementGroup);
            await database.SaveChangesAsync();

            // for each location store location group + samples
            foreach (var locEntry in aggregatedByLocation)
            {
                var locHash = locEntry.Key;
                var dbLocation = await database.Locations.FirstOrDefaultAsync(l => l.Hash == locHash);

                if (dbLocation is null)
                {
                    logger.LogWarning($"Location with hash '{locHash}' was not recognized!");
                    continue;
                }

                var isLocationRequested = context.Message.Raport.RequestedLocations.Any(x => x.Hash == locHash);
                if (isLocationRequested == false)
                {
                    logger.LogWarning($"Location '{dbLocation.Name}' was not requested for this raport, skipping.", locHash);
                    continue;
                }
                else
                {
                    logger.LogWarning($"Creating Location Group for '{dbLocation.Name}'", locHash);
                }

                //  Append new location group to DB!
                var locationGroup = new LocationGroup
                {
                    LocationID = dbLocation.ID,
                    MeasurementGroupID = measurementGroup.ID,
                    Summary = string.Empty
                };

                await database.LocationGroups.AddAsync(locationGroup);
                await database.SaveChangesAsync();

                //  Create samples
                var samples = new List<SampleGroup>();
                foreach (var bucket in locEntry.Value)
                {
                    var val = GetMeasurementValueByKey(bucket, key);
                    if (val.HasValue)
                    {
                        samples.Add(new SampleGroup { Date = bucket.TimePoint, Value = val.Value, LocationGroupID = locationGroup.ID });
                    }
                }

                if (samples.Count > 0)
                {
                    await database.SampleGroups.AddRangeAsync(samples);
                    await database.SaveChangesAsync();
                }
            }
        }

        logger.LogInformation($"Hourly raport processed and saved RaportID={raport.ID}");

        //  Forward to next step
        var validateRaportMessage = new ValidateRaport()
        {
            Raport = context.Message.Raport
        };

        await publish.Publish(validateRaportMessage, ct);

        logger.LogInformation($"Raport sent to validation!");
    }

    private static double? GetMeasurementValueByKey(AggregatedBucket bucket, string key)
    {
        return key switch
        {
            "Temperature" => bucket.Temperature,
            "Humidity" => bucket.Humidity,
            "CarbonDioxide" => bucket.CarbonDioxide,
            "VolatileOrganicCompounds" => bucket.VolatileOrganicCompounds,
            "ParticulateMatter1" => bucket.ParticulateMatter1,
            "ParticulateMatter2v5" => bucket.ParticulateMatter2v5,
            "ParticulateMatter10" => bucket.ParticulateMatter10,
            "Formaldehyde" => bucket.Formaldehyde,
            "CarbonMonoxide" => bucket.CarbonMonoxide,
            "Ozone" => bucket.Ozone,
            "Ammonia" => bucket.Ammonia,
            "Airflow" => bucket.Airflow,
            "AirIonizationLevel" => bucket.AirIonizationLevel,
            "Oxygen" => bucket.Oxygen,
            "Radon" => bucket.Radon,
            "Illuminance" => bucket.Illuminance,
            "SoundLevel" => bucket.SoundLevel,
            _ => null
        };
    }

    private sealed class AggregatedBucket
    {
        public DateTime TimePoint { get; }
        public DateTime WindowStart { get; }
        public DateTime WindowEnd { get; }
        public double? Temperature { get; }
        public double? Humidity { get; }
        public double? CarbonDioxide { get; }
        public double? VolatileOrganicCompounds { get; }
        public double? ParticulateMatter1 { get; }
        public double? ParticulateMatter2v5 { get; }
        public double? ParticulateMatter10 { get; }
        public double? Formaldehyde { get; }
        public double? CarbonMonoxide { get; }
        public double? Ozone { get; }
        public double? Ammonia { get; }
        public double? Airflow { get; }
        public double? AirIonizationLevel { get; }
        public double? Oxygen { get; }
        public double? Radon { get; }
        public double? Illuminance { get; }
        public double? SoundLevel { get; }
        public int Count { get; }

        public AggregatedBucket(DateTime timePoint, DateTime windowStart, DateTime windowEnd,
            double? temperature, double? humidity, double? carbonDioxide, double? volatileOrganicCompounds,
            double? particulateMatter1, double? particulateMatter2v5, double? particulateMatter10,
            double? formaldehyde, double? carbonMonoxide, double? ozone, double? ammonia,
            double? airflow, double? airIonizationLevel, double? oxygen, double? radon,
            double? illuminance, double? soundLevel, int count)
        {
            TimePoint = timePoint;
            WindowStart = windowStart;
            WindowEnd = windowEnd;
            Temperature = temperature;
            Humidity = humidity;
            CarbonDioxide = carbonDioxide;
            VolatileOrganicCompounds = volatileOrganicCompounds;
            ParticulateMatter1 = particulateMatter1;
            ParticulateMatter2v5 = particulateMatter2v5;
            ParticulateMatter10 = particulateMatter10;
            Formaldehyde = formaldehyde;
            CarbonMonoxide = carbonMonoxide;
            Ozone = ozone;
            Ammonia = ammonia;
            Airflow = airflow;
            AirIonizationLevel = airIonizationLevel;
            Oxygen = oxygen;
            Radon = radon;
            Illuminance = illuminance;
            SoundLevel = soundLevel;
            Count = count;
        }
    }
}
