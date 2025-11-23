namespace Devices.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Migrate DB - use only for Dev DB!
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DevicesDBContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();
        }
    }
}
