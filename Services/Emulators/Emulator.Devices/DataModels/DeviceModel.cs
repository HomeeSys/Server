namespace Emulator.Devices.DataModels;

internal class DeviceModel
{
    public string Name { get; set; } = default!;
    public Guid DeviceNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public LocationModel Location { get; set; } = default!;
    public TimestampConfigurationModel Timestamp { get; set; } = default!;
    public StatusModel Status { get; set; } = default!;
    public MeasurementConfigurationModel Configuration { get; set; } = default!;

    public bool IsEqual(DeviceModel other)
    {
        if (DeviceNumber != other.DeviceNumber)
        {
            return false;
        }

        return Location.IsEqual(other.Location) && Timestamp.IsEqual(other.Timestamp) && Status.IsEqual(other.Status) && Configuration.IsEqual(other.Configuration);
    }

    public void Update(DeviceModel other)
    {
        Name = other.Name;
        DeviceNumber = other.DeviceNumber;
        RegisterDate = other.RegisterDate;
        Location.Update(other.Location);
        Timestamp.Update(other.Timestamp);
        Status.Update(other.Status);
        Configuration.Update(other.Configuration);
    }

    public bool CanGenerateMeasurementSet()
    {
        return Configuration.Temperature || Configuration.Humidity || Configuration.CarbonDioxide || Configuration.VolatileOrganicCompounds ||
            Configuration.PM1 || Configuration.PM25 || Configuration.PM10 || Configuration.Formaldehyde ||
            Configuration.CarbonMonoxide || Configuration.Ozone || Configuration.Ammonia || Configuration.Airflow ||
            Configuration.AirIonizationLevel || Configuration.Oxygen || Configuration.Radon || Configuration.Illuminance || Configuration.SoundLevel;
    }

    public MeasurementSetModel GenerateMeasurementSet()
    {
        var measurementSet = new MeasurementSetModel
        {
            DeviceNumber = DeviceNumber,
            RegisterDate = DateTime.Now
        };

        if (Configuration.Temperature) measurementSet.Temperature = CreateMeasurement(20, 24, "°C");

        if (Configuration.Humidity) measurementSet.Humidity = CreateMeasurement(35, 60, "%");

        if (Configuration.CarbonDioxide) measurementSet.CO2 = CreateMeasurement(400, 1000, "ppm");

        if (Configuration.VolatileOrganicCompounds) measurementSet.VOC = CreateMeasurement(50, 300, "µg/m³");

        if (Configuration.PM1) measurementSet.ParticulateMatter1 = CreateMeasurement(0, 10, "µg/m³");

        if (Configuration.PM25) measurementSet.ParticulateMatter2v5 = CreateMeasurement(0, 15, "µg/m³");

        if (Configuration.PM10) measurementSet.ParticulateMatter10 = CreateMeasurement(0, 45, "µg/m³");

        if (Configuration.Formaldehyde) measurementSet.Formaldehyde = CreateMeasurement(0, 100, "µg/m³");

        if (Configuration.CarbonMonoxide) measurementSet.CO = CreateMeasurement(0, 9, "ppm");

        if (Configuration.Ozone) measurementSet.O3 = CreateMeasurement(0, 100, "µg/m³");

        if (Configuration.Ammonia) measurementSet.Ammonia = CreateMeasurement(0, 0.2, "ppm");

        if (Configuration.Airflow) measurementSet.Airflow = CreateMeasurement(0.05, 0.2, "m/s");

        if (Configuration.AirIonizationLevel) measurementSet.AirIonizationLevel = CreateMeasurement(500, 50000, "ions/cm³");

        if (Configuration.Oxygen) measurementSet.O2 = CreateMeasurement(19.5, 23.5, "%");

        if (Configuration.Radon) measurementSet.Radon = CreateMeasurement(0, 300, "Bq/m³");

        if (Configuration.Illuminance) measurementSet.Illuminance = CreateMeasurement(200, 1000, "lx");

        if (Configuration.SoundLevel) measurementSet.SoundLevel = CreateMeasurement(30, 55, "dB(A)");

        return measurementSet;
    }

    private MeasurementModel CreateMeasurement(double min, double max, string unit)
    {
        Random rnd = new Random();
        double value = rnd.NextDouble() * (max - min) + min;
        return new MeasurementModel
        {
            Value = Math.Round(value, 2),
            Unit = unit
        };
    }
}
