namespace Measurements.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        TypeAdapterConfig<Measurement, DefaultMeasurementDTO>
            .NewConfig()
            .Map(x => x.ParticulateMatter2v5, y => y.ParticulateMatter2v5)
            .Map(x => x.LocationID, y => y.LocationID)
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<DefaultMeasurementDTO, Measurement>
            .NewConfig()
            .Map(x => x.ParticulateMatter2v5, y => y.ParticulateMatter2v5)
            .Map(x => x.LocationID, y => y.LocationID)
            .Map(x => x.ID, y => y.ID);

        services.AddCarter();

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(ValidationBehavior<,>));
            x.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddGrpcClient<DevicesService.DevicesServiceClient>(options =>
        {
            options.Address = new Uri(configuration.GetConnectionString("DevicesGRPC"));
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
        });

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddSignalR();

        return services;
    }

    public static WebApplication AddApplicationServicesUsage(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(x => { });

        app.MapHub<MeasurementHub>("/measurementhub");

        return app;
    }
}
