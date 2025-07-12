namespace Devices.Infrastructure
{
    public static class DependancyInjection
    {
        /// <summary>
        /// Dependnecy injection for this `Devices.Infrastructure` project.
        /// </summary>
        /// <param name="services">API Services</param>
        /// <param name="config">API Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            var connStr = config.GetConnectionString("DevicesDB");

            services.AddDbContext<DevicesDBContext>(options =>
            {
                options.UseSqlServer(connStr);
            });

            return services;
        }
    }
}
