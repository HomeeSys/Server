namespace Devices.Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            string connectionString = string.Empty;
            if (environment.IsDevelopment())
            {
                connectionString = config.GetConnectionString("DevicesDB_Dev");
            }
            else if (environment.IsStaging())
            {
                connectionString = config.GetConnectionString("DevicesDB_Dev");
            }
            else
            {
                connectionString = config.GetConnectionString("DevicesDB_Prod");
            }

            services.AddDbContext<DevicesDBContext>(options =>
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
}
