namespace Emulators.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<EmulatorsDBContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await DropCurrent(context);
        await SeedAsync(context);
    }

    private static async Task DropCurrent(EmulatorsDBContext context)
    {
        var tables = new string[]
        {
            nameof(context.ChartOffsets),
            nameof(context.Devices),
            nameof(context.Locations),
            nameof(context.Samples),
            nameof(context.ChartTemplates),
        };

        foreach (var table in tables)
        {
            await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
            await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('[{table}]', RESEED, 0)");
        }
    }

    private static async Task SeedAsync(EmulatorsDBContext context)
    {
        //  TODO: execute sql script
    }
}
