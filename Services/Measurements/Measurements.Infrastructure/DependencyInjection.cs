using Microsoft.AspNetCore.Builder;

namespace Measurements.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<MeasurementsDBContext>();

        return services;
    }

    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication webapp)
    {
        using var scope = webapp.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<MeasurementsDBContext>();
        await context.InitializeDatabase();

        return webapp;
    }
}