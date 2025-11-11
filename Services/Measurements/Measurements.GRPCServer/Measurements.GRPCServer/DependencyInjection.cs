namespace Measurements.GRPCServer;

public static class DependencyInjection
{
    public static IServiceCollection AddGRPCServerServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddGrpc();

        return services;
    }

    public static WebApplication AddGRPCServerServicesUsage(this WebApplication app)
    {
        app.MapGrpcService<MeasurementsServerService>();

        return app;
    }
}
