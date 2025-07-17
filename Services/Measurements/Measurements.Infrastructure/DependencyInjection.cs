namespace Measurements.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<MeasurementsDBContext>();
        
        return services;
    }
}