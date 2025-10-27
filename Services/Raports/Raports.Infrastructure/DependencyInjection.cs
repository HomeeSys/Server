namespace Raports.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connStr = config.GetConnectionString("RaportsDB");

            services.AddDbContext<RaportsDBContext>(options =>
            {
                options.UseSqlServer(connStr, sqlOptions =>
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
