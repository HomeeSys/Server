using CommonServiceLibrary.Messaging;

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

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        services.AddSingleton(config);

        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }

    public static WebApplication AddApplicationServicesUsage(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(x => { });

        return app;
    }
}
