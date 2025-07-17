namespace Measurements.GRPC;
public static class DependencyInjection
{
    public static IServiceCollection AddGRPCServices(this IServiceCollection services)
    {
        services.AddScoped<MeasurementsDBContext>();
        services.AddGrpc();

        //  Mappster mapping fix.
        var config = new TypeAdapterConfig();

        config.Apply(new MeasurementGRPCMapper());

        return services;
    }

    public static WebApplication AddGRPCServicesUsage(this WebApplication app)
    {
        app.MapGrpcService<MeasurementSetService>();

        return app;
    }
}
