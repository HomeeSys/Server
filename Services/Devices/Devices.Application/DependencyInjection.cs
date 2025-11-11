namespace Devices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        TypeAdapterConfig<LocationDTO, Location>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<Location, LocationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<TimestampConfigurationDTO, TimestampConfiguration>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<TimestampConfiguration, TimestampConfigurationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Cron, y => y.Cron);

        TypeAdapterConfig<DefaultDeviceDTO, Device>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<Device, DefaultDeviceDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<DefaultMeasurementConfigurationDTO, MeasurementConfiguration>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<MeasurementConfiguration, DefaultMeasurementConfigurationDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<StatusDTO, Status>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<Status, StatusDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID);

        services.AddCarter();

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(ValidationBehavior<,>));
            x.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        services.AddSignalR();

        return services;
    }

    public static WebApplication AddApplicationServicesUsage(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(x => { });

        app.MapHub<DeviceHub>("/devicehub");

        return app;
    }
}
