using CommonServiceLibrary.Messaging.Messages.MeasurementsService;

namespace Emulators.Application.Consumers;

internal class DeviceGenerateMeasurementConsumer(ILogger<DeviceGenerateMeasurementConsumer> logger, EmulatorsDBContext database, IMemoryCache cashe, IPublishEndpoint publisher) : IConsumer<DeviceGenerateMeasurement>
{
    public async Task Consume(ConsumeContext<DeviceGenerateMeasurement> context)
    {
        var sw = Stopwatch.StartNew();

        var deviceArg = context.Message.Device;
        var dateArg = context.Message.Date;
        var targetTime = new TimeOnly(dateArg.Hour, dateArg.Minute, dateArg.Second, dateArg.Millisecond, dateArg.Microsecond);
        var targetDate = new DateOnly(dateArg.Year, dateArg.Month, dateArg.Day);

        //  Device from cashe
        var cashedDevice = cashe.Get<Device>($"{nameof(Device)}:{deviceArg.DeviceNumber}");
        if (cashedDevice is null)
        {
            //  Doesn't exist in cashe
            var dbDevice = await database.Devices.FirstOrDefaultAsync(x => x.DeviceNumber == deviceArg.DeviceNumber);
            if (dbDevice is null)
            {
                logger.LogError($"{nameof(deviceArg)} - Failed to find device: '{deviceArg.DeviceNumber}'");
                return;
            }

            cashe.Set($"{nameof(Device)}:{deviceArg.DeviceNumber}", dbDevice);
            logger.LogInformation($"{nameof(DeviceGenerateMeasurementConsumer)} - {nameof(Device)} added to cashe: '{dbDevice.DeviceNumber}'");

            cashedDevice = dbDevice;
        }
        else
        {
            //logger.LogInformation($"{nameof(CreateMeasurementJob)} - {nameof(Device)} retrieved from cashe: '{cashedDevice.DeviceNumber}'");
        }

        //  Location from cashe
        var cashedLocation = cashe.Get<Location>($"{nameof(Location)}:{deviceArg.Location.Name}");
        if (cashedLocation is null)
        {
            //  Doesn't exist in cashe
            var dbLocation = await database.Locations.FirstOrDefaultAsync(x => x.Name == deviceArg.Location.Name);
            if (dbLocation is null)
            {
                logger.LogError($"{nameof(DeviceGenerateMeasurementConsumer)} - Failed to find location: '{deviceArg.Location.Name}'");
                return;
            }

            cashe.Set($"{nameof(Location)}:{deviceArg.Location.Name}", dbLocation);
            logger.LogInformation($"{nameof(DeviceGenerateMeasurementConsumer)} - {nameof(Location)} added to cashe: '{dbLocation.Name}'");

            cashedLocation = dbLocation;
        }
        else
        {
            //logger.LogInformation($"{nameof(CreateMeasurementJob)} - {nameof(Location)} retrieved from cashe: '{cashedLocation}'");
        }

        //  Measurement type from cashe
        var cashedMeasurements = new List<Measurement>();
        foreach (var mes in deviceArg.MeasurementTypes)
        {
            var cashedMeasurement = cashe.Get<Measurement>($"{nameof(Measurement)}:{mes.Name}");
            if (cashedMeasurement is null)
            {
                var dbMeasurement = await database.Measurements.FirstOrDefaultAsync(x => x.Name == mes.Name);
                if (dbMeasurement is null)
                {
                    logger.LogError($"{nameof(DeviceGenerateMeasurementConsumer)} - Failed to find measurement: '{mes.Name}'");
                    return;
                }

                cashe.Set($"{nameof(Measurement)}:{dbMeasurement.Name}", dbMeasurement);
                logger.LogInformation($"{nameof(DeviceGenerateMeasurementConsumer)} - {nameof(Measurement)} added to cashe: '{dbMeasurement.Name}'");

                cashedMeasurement = dbMeasurement;
            }
            else
            {
                //logger.LogInformation($"{nameof(CreateMeasurementJob)} - {nameof(Measurement)} retrieved from cashe: '{cashedLocation}'");
            }

            cashedMeasurements.Add(cashedMeasurement);
        }

        //  ChartOffset with ChartTemplate from cashe
        var cashedChartOffsets = new List<ChartOffset>();
        foreach (var mes in cashedMeasurements)
        {
            var cashedChartOffset = cashe.Get<ChartOffset>($"{nameof(ChartOffset)}:{mes.Name}:{deviceArg.Location.Name}");
            if (cashedChartOffset is null)
            {
                var dbChartOffset = await database.ChartOffsets
                    .Include(cho => cho.Location)
                    .Include(cho => cho.ChartTemplate)
                        .ThenInclude(ct => ct.Measurement)
                    .Include(cho => cho.ChartTemplate)
                        .ThenInclude(ct => ct.Samples)
                    .Where(cho =>
                        cho.Location.Name == deviceArg.Location.Name &&
                        cho.ChartTemplate.Measurement.Name == mes.Name)
                    .FirstOrDefaultAsync();
                if (dbChartOffset is null)
                {
                    logger.LogError($"{nameof(ChartOffset)} - Failed to find {nameof(ChartOffset)} for measurement type: '{mes.Name}' and location '{deviceArg.Location.Name}'");
                    return;
                }

                cashe.Set($"{nameof(ChartOffset)}:{mes.Name}:{deviceArg.Location.Name}", dbChartOffset);
                logger.LogInformation($"{nameof(DeviceGenerateMeasurementConsumer)} - {nameof(ChartOffset)}  for measurement type: '{mes.Name}' and location '{deviceArg.Location.Name}' added to cashe!");

                cashedChartOffset = dbChartOffset;
            }
            else
            {
                //logger.LogInformation($"{nameof(CreateMeasurementJob)} - {nameof(ChartOffset)} retrieved from cashe: '{cashedLocation}'");
            }
            cashedChartOffsets.Add(cashedChartOffset);
        }

        /*  We have to go throught all of measurements for this device and get two dates, one before and one after date for whitch measurement is generated.
         *  
         */

        double? airTemperature = default;
        double? relativeHumidity = default;
        double? carbonDioxide = default;
        double? voc = default;
        double? pm1 = default;
        double? pm25 = default;
        double? pm10 = default;
        double? formaldehyde = default;
        double? carbonMonixide = default;
        double? ozone = default;
        double? ammonia = default;
        double? airFlowRate = default;
        double? airIonizationLevel = default;
        double? oxygen = default;
        double? radon = default;
        double? illuminance = default;
        double? sound = default;

        foreach (var chartOffset in cashedChartOffsets)
        {
            var chart = chartOffset.ChartTemplate;

            //  Get one time before and one after.
            var previousSample = chart.Samples
                .Where(s => s.Time < targetTime)
                .OrderByDescending(s => s.Time)
                .FirstOrDefault();

            if (previousSample is null)
            {
                //  Get last one 
                previousSample = chart.Samples
                    .OrderByDescending(x => x.Time)
                    .FirstOrDefault();
            }

            var nextSample = chart.Samples
                .Where(s => s.Time > targetTime)
                .OrderBy(s => s.Time)
                .FirstOrDefault();

            //  It is the 00:00
            if (nextSample is null)
            {
                nextSample = chart.Samples
                    .OrderBy(x => x.Time)
                    .FirstOrDefault();
            }

            var seconds = targetTime.Minute * 60 + targetTime.Second;
            var deltaValue = nextSample.Value - previousSample.Value;
            var deltaValuePerSec = deltaValue / (60 * 60);
            var valueForTime = seconds * deltaValuePerSec;
            var newValue = previousSample.Value + valueForTime;

            var offsetedValue = newValue + chartOffset.Value;

            //  For offseted value we have to modify if by device spread
            var spreadValue = offsetedValue * cashedDevice.Spread / 100;
            var minValue = offsetedValue - spreadValue;
            var maxValue = offsetedValue + spreadValue;

            //  Generate random value with boundaries
            Random random = new Random();
            var deviceSpreadedValue = Math.Round(minValue + random.NextDouble() * (maxValue - minValue), 2);

            switch (chart.Measurement.Name)
            {
                case "Air Temperature":
                    airTemperature = deviceSpreadedValue;
                    break;
                case "Relative Humidity":
                    relativeHumidity = deviceSpreadedValue;
                    break;
                case "Carbon Dioxide":
                    carbonDioxide = deviceSpreadedValue;
                    break;
                case "Volatile Organic Compounds":
                    voc = deviceSpreadedValue;
                    break;
                case "Particulate Matter 1um":
                    pm1 = deviceSpreadedValue;
                    break;
                case "Particulate Matter 2.5um":
                    pm25 = deviceSpreadedValue;
                    break;
                case "Particulate Matter 10um":
                    pm10 = deviceSpreadedValue;
                    break;
                case "Formaldehyde":
                    formaldehyde = deviceSpreadedValue;
                    break;
                case "Carbon Monoxide":
                    carbonMonixide = deviceSpreadedValue;
                    break;
                case "Ozone":
                    ozone = deviceSpreadedValue;
                    break;
                case "Ammonia":
                    ammonia = deviceSpreadedValue;
                    break;
                case "Air Flow Rate":
                    airFlowRate = deviceSpreadedValue;
                    break;
                case "Air Ionization Level":
                    airIonizationLevel = deviceSpreadedValue;
                    break;
                case "Oxygen Concentration":
                    oxygen = deviceSpreadedValue;
                    break;
                case "Radon Concentration":
                    radon = deviceSpreadedValue;
                    break;
                case "Illuminance level":
                    illuminance = deviceSpreadedValue;
                    break;
                case "Sound Pressure Level":
                    sound = deviceSpreadedValue;
                    break;
                default:
                    logger.LogError($"Unhandled assingment of generated value for '{chart.Measurement.Name}'");
                    break;
            }
        }
        sw.Stop();


        var creationDate = dateArg.AddMinutes(sw.ElapsedMilliseconds);

        var createMeasurement = new MeasurementsMessage_CreateMeasurement(Guid.NewGuid(), deviceArg.DeviceNumber, creationDate, deviceArg.Location.Hash, airTemperature, relativeHumidity,
            carbonDioxide, voc, pm1, pm25, pm10, formaldehyde, carbonDioxide, ozone, ammonia, airFlowRate, airIonizationLevel, oxygen, radon, illuminance, sound);

        var topicMessage = new CreateMeasurement()
        {
            Measurement = createMeasurement,
        };

        await publisher.Publish(topicMessage);

        logger.LogInformation($"Device '{deviceArg.Name}' generated measurement for '{creationDate}'");
    }
}