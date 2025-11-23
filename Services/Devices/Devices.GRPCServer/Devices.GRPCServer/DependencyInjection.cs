namespace Devices.GRPCServer;

public static class DependencyInjection
{
    public static IServiceCollection AddGRPCServerServices(this IServiceCollection services, IConfiguration config)
    {
        TypeAdapterConfig<Device, GrpcDeviceModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate)
            .Map(dest => dest.LocationId, src => src.LocationID)
            .Map(dest => dest.Location, src => src.Location)
            .Map(dest => dest.TimestampConfigurationId, src => src.TimestampID)
            .Map(dest => dest.TimestampConfiguration, src => src.Timestamp)
            .Map(dest => dest.StatusId, src => src.StatusID)
            .Map(dest => dest.Status, src => src.Status);

        TypeAdapterConfig<Location, GrpcLocationModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Name, src => src.Name);

        TypeAdapterConfig<Domain.Models.Status, GrpcStatusModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Type, src => src.Type);

        //TypeAdapterConfig<MeasurementConfiguration, GrpcMeasurementConfiguration>
        //    .NewConfig()
        //    .Map(dest => dest.Id, src => src.ID)
        //    .Map(dest => dest.Temperature, src => src.Temperature)
        //    .Map(dest => dest.Humidity, src => src.Humidity)
        //    .Map(dest => dest.CarbonDioxide, src => src.CarbonDioxide)
        //    .Map(dest => dest.VolatileOrganicCompounds, src => src.VolatileOrganicCompounds)
        //    .Map(dest => dest.Pm1, src => src.PM1)
        //    .Map(dest => dest.Pm25, src => src.PM25)
        //    .Map(dest => dest.Pm10, src => src.PM10)
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

        TypeAdapterConfig<Timestamp, GrpcTimestampConfigurationModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Cron, src => src.Cron);

        services.AddGrpc();

        return services;
    }

    public static WebApplication AddGRPCServerServicesUsage(this WebApplication app)
    {
        app.MapGrpcService<DevicesServerService>();

        return app;
    }
}
