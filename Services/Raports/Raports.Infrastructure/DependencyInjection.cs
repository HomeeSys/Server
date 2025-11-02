namespace Raports.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            string connectionString = string.Empty;
            if (environment.IsDevelopment())
            {
                connectionString = config.GetConnectionString("RaportsDB_Dev");
            }
            else if (environment.IsStaging())
            {
                connectionString = config.GetConnectionString("RaportsDB_Dev");
            }
            else
            {
                connectionString = config.GetConnectionString("RaportsDB_Prod");
            }

            services.AddDbContext<RaportsDBContext>(options =>
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
