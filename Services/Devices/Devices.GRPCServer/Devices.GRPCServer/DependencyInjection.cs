namespace Devices.GRPCServer;

public static class DependencyInjection
{
    public static IServiceCollection AddGRPCServerServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddGrpc();

        TypeAdapterConfig<Device, GrpcDeviceModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.LocationId, src => src.LocationID)
            .Map(dest => dest.Location, src => src.Location)
            .Map(dest => dest.TimestampConfigurationId, src => src.TimestampConfigurationID)
            .Map(dest => dest.TimestampConfiguration, src => src.TimestampConfiguration)
            .Map(dest => dest.MeasurementConfigurationId, src => src.MeasurementConfigurationID)
            .Map(dest => dest.MeasurementConfiguration, src => src.MeasurementConfiguration)
            .Map(dest => dest.StatusId, src => src.StatusID)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"));

        TypeAdapterConfig<TimestampConfiguration, GrpcTimestampConfigurationModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Cron, src => src.Cron);

        TypeAdapterConfig<MeasurementConfiguration, GrpcMeasurementConfiguration>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID);

        TypeAdapterConfig<Location, GrpcLocationModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID);

        TypeAdapterConfig<Domain.Models.Status, GrpcStatusModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID);

        return services;
    }

    public static WebApplication AddGRPCServerServicesUsage(this WebApplication app)
    {
        app.MapGrpcService<DevicesServerService>();

        return app;
    }
}
