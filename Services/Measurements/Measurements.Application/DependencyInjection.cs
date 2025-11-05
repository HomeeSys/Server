using Devices.GRPCClient;

namespace Measurements.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            x.AddOpenBehavior(typeof(ValidationBehavior<,>));
            x.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        var config = new TypeAdapterConfig();

        config.Apply(new MeasurementMapper());

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

        services.AddSingleton(config);

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
