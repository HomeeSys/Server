namespace Devices.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Updates DB automatically.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<DevicesDBContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();

            await SeedAsync(context);
        }

        private static async Task SeedAsync(DevicesDBContext context)
        {
            foreach (var entityType in context.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{tableName}]");
                await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('[{tableName}]', RESEED, 0)");
            }

            await SeedStatusesAsync(context);
            await SeedTimestampConfigurationsAsync(context);
            await SeedLocationsAsync(context);
            await SeedDevicesAsync(context);
            await SeedMeasurementConfigsAsync(context);
        }

        private static async Task SeedDevicesAsync(DevicesDBContext context)
        {
            if (!await context.Devices.AnyAsync())
            {
                await context.Devices.AddRangeAsync(DevicesSeed.Devices);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedLocationsAsync(DevicesDBContext context)
        {
            if (!await context.Locations.AnyAsync())
            {
                await context.Locations.AddRangeAsync(LocationsSeed.Locations);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedMeasurementConfigsAsync(DevicesDBContext context)
        {
            if (!await context.MeasurementConfigs.AnyAsync())
            {
                var devices = await context.Devices.ToListAsync();

                List<MeasurementConfig> configs = new List<MeasurementConfig>();
                foreach (var device in devices)
                {
                    MeasurementConfig newConfig = new MeasurementConfig() { DeviceId = device.Id, Device = device };
                    configs.Add(newConfig);
                }

                await context.MeasurementConfigs.AddRangeAsync(configs);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedTimestampConfigurationsAsync(DevicesDBContext context)
        {
            if (!await context.TimestampConfigurations.AnyAsync())
            {
                await context.TimestampConfigurations.AddRangeAsync(TimestampConfigurationsSeed.Timestamps);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedStatusesAsync(DevicesDBContext context)
        {
            if (!await context.Statuses.AnyAsync())
            {
                await context.Statuses.AddRangeAsync(StatusesSeed.Statuses);
                await context.SaveChangesAsync();
            }
        }
    }
}
