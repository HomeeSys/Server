namespace Raports.Infrastructure.Extensions;

public static class RaportsDatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<RaportsDBContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(RaportsDBContext context)
    {
        var tables = new string[]
        {
            nameof(context.Raports),
            nameof(context.Requests),
            nameof(context.Periods),
            nameof(context.RequestStatuses),
        };

        foreach (var table in tables)
        {
            await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
            await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('[{table}]', RESEED, 0)");
        }

        await SeedPeriodsAsync(context);
        await SeedStatusedAsync(context);
        await SeedRequestsAsync(context);
        await SeedRaportsAsync(context);
    }

    private static async Task SeedPeriodsAsync(RaportsDBContext context)
    {
        if (!await context.Periods.AnyAsync())
        {
            IEnumerable<Period> periodsTestSet = new List<Period>()
            {
                new Period(){ Name = "Hourly", Hours = 1 },
                new Period(){ Name = "Daily", Hours = 1 * 24 },
                new Period(){ Name = "Weekly", Hours = 7 * 24 },
                //  new Period(){ Name = "Monthly", Hours = ? },    //  For now i will remove that becuase i dont really know what to put into hours.
            };

            await context.Periods.AddRangeAsync(periodsTestSet);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedStatusedAsync(RaportsDBContext context)
    {
        if (!await context.RequestStatuses.AnyAsync())
        {
            IEnumerable<RequestStatus> statusesTestSet = new List<RequestStatus>()
            {
                new RequestStatus(){ Name = "Pending", Description = "This request is waiting for it's computation" },
                new RequestStatus(){ Name = "Completed", Description = "Request was created successfully" },
                new RequestStatus(){ Name = "Failed", Description = "Data for this report is not sufficient" },
            };

            await context.RequestStatuses.AddRangeAsync(statusesTestSet);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRequestsAsync(RaportsDBContext context)
    {
        if (!await context.Requests.AnyAsync())
        {
            IEnumerable<Request> requestTestSet = new List<Request>()
            {
                //new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1), StatusID = 1, PeriodID = 1 },
                //new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(2), StatusID = 2, PeriodID = 2 },
                //new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-3), EndDate = DateTime.Now.AddDays(3), StatusID = 2, PeriodID = 1 },
            };

            await context.Requests.AddRangeAsync(requestTestSet);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRaportsAsync(RaportsDBContext context)
    {
        if (!await context.Raports.AnyAsync())
        {
            IEnumerable<Raport> raportsTestSet = new List<Raport>()
            {
                //new Raport(){ CreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1), PeriodID = 1, RequestID = 1 },
            };

            await context.Raports.AddRangeAsync(raportsTestSet);
            await context.SaveChangesAsync();
        }
    }
}
