namespace Emulators.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        string connectionString = string.Empty;
        if (environment.IsDevelopment())
        {
            connectionString = configuration.GetConnectionString("EmulatorsDB_Dev");
        }
        else if (environment.IsStaging())
        {
            connectionString = configuration.GetConnectionString("EmulatorsDB_Dev");
        }
        else
        {
            connectionString = configuration.GetConnectionString("EmulatorsDB_Prod");
        }

        services.AddDbContext<EmulatorsDBContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                );
            });
        });

        return services;
    }
}
