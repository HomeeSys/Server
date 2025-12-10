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

        TypeAdapterConfig<Timestamp, GrpcTimestampConfigurationModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.Cron, src => src.Cron);

        services.AddGrpc();

        return services;
    }

    public static WebApplication AddGRPCServerServicesUsage(this WebApplication app)
    {
        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        app.MapGrpcService<DevicesServerService>();

        return app;
    }
}
