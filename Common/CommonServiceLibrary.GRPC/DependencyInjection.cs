namespace CommonServiceLibrary.GRPC;

public static class DependencyInjection
{
    public static IServiceCollection AddGRPCMappings(this IServiceCollection services)
    {
        //  Devices
        TypeAdapterConfig<GrpcDeviceModel, Device>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate)
            .Map(dest => dest.LocationID, src => src.LocationId)
            .Map(dest => dest.Location, src => src.Location)
            .Map(dest => dest.TimestampID, src => src.TimestampConfigurationId)
            .Map(dest => dest.Timestamp, src => src.TimestampConfiguration)
            .Map(dest => dest.StatusID, src => src.StatusId)
            .Map(dest => dest.Status, src => src.Status);

        TypeAdapterConfig<GrpcTimestampConfigurationModel, Timestamp>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Cron, src => src.Cron);

        TypeAdapterConfig<GrpcLocationModel, Location>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Name, src => src.Name);

        TypeAdapterConfig<GrpcStatusModel, Devices.Domain.Models.Status>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.Type, src => src.Type);

        //TypeAdapterConfig<GrpcMeasurementConfiguration, MeasurementConfiguration>
        //    .NewConfig()
        //    .Map(dest => dest.ID, src => src.Id)
        //    .Map(dest => dest.Temperature, src => src.Temperature)
        //    .Map(dest => dest.Humidity, src => src.Humidity)
        //    .Map(dest => dest.CarbonDioxide, src => src.CarbonDioxide)
        //    .Map(dest => dest.VolatileOrganicCompounds, src => src.VolatileOrganicCompounds)
        //    .Map(dest => dest.PM1, src => src.Pm1)
        //    .Map(dest => dest.PM25, src => src.Pm25)
        //    .Map(dest => dest.PM10, src => src.Pm10)
        //    .Map(dest => dest.Formaldehyde, src => src.Formaldehyde)
        //    .Map(dest => dest.CarbonMonoxide, src => src.CarbonMonoxide)
        //    .Map(dest => dest.Ozone, src => src.Ozone)
        //    .Map(dest => dest.Ammonia, src => src.Ammonia)
        //    .Map(dest => dest.Airflow, src => src.Airflow)
        //    .Map(dest => dest.AirIonizationLevel, src => src.AirIonizationLevel)
        //    .Map(dest => dest.Oxygen, src => src.Oxygen)
        //    .Map(dest => dest.Radon, src => src.Radon)
        //    .Map(dest => dest.Illuminance, src => src.Illuminance)
        //    .Map(dest => dest.SoundLevel, src => src.SoundLevel);

        ////  Meaesurements
        //TypeAdapterConfig<MeasurementGrpcModel, Measurement>
        //    .NewConfig()
        //    .Map(dest => dest.ID, src => src.Id)
        //    .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
        //    .Map(dest => dest.Temperature, src => src.Temperature)
        //    .Map(dest => dest.Humidity, src => src.Humidity)
        //    .Map(dest => dest.CarbonDioxide, src => src.Co2)
        //    .Map(dest => dest.VolatileOrganicCompounds, src => src.Voc)
        //    .Map(dest => dest.ParticulateMatter1, src => src.ParticulateMatter1)
        //    .Map(dest => dest.ParticulateMatter2v5, src => src.ParticulateMatter2V5)
        //    .Map(dest => dest.ParticulateMatter10, src => src.ParticulateMatter10)
        //    .Map(dest => dest.Formaldehyde, src => src.Formaldehyde)
        //    .Map(dest => dest.CarbonMonoxide, src => src.Co)
        //    .Map(dest => dest.Ammonia, src => src.Ammonia)
        //    .Map(dest => dest.Airflow, src => src.Airflow)
        //    .Map(dest => dest.AirIonizationLevel, src => src.AirIonizationLevel)
        //    .Map(dest => dest.Oxygen, src => src.O2)
        //    .Map(dest => dest.Radon, src => src.Radon)
        //    .Map(dest => dest.Illuminance, src => src.Illuminance)
        //    .Map(dest => dest.SoundLevel, src => src.SoundLevel);

        return services;
    }
}
