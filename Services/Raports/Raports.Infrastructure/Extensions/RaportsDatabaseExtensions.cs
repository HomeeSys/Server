//namespace Raports.Infrastructure.Extensions;

//public static class RaportsDatabaseExtensions
//{
//    public static async Task InitializeDatabaseAsync(this WebApplication app)
//    {
//        using var scope = app.Services.CreateScope();

//        var context = scope.ServiceProvider.GetRequiredService<RaportsDBContext>();

//        context.Database.MigrateAsync().GetAwaiter().GetResult();

//        await SeedAsync(context);
//    }

//    private static async Task SeedAsync(RaportsDBContext context)
//    {
//        var tables = new string[]
//        {
//            nameof(context.Raports),
//            nameof(context.Requests),
//            nameof(context.Periods),
//            nameof(context.RequestStatuses),
//        };

//        foreach (var table in tables)
//        {
//            await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
//            await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('[{table}]', RESEED, 0)");
//        }

//        await SeedPeriodsAsync(context);
//        await SeedStatusedAsync(context);
//        await SeedRequestsAsync(context);
//        await SeedRaportsAsync(context);
//    }

//    private static async Task SeedPeriodsAsync(RaportsDBContext context)
//    {
//        if (!await context.Periods.AnyAsync())
//        {
//            IEnumerable<Period> periodsTestSet = new List<Period>()
//            {
//                new Period(){ Name = "Hourly" },
//                new Period(){ Name = "Daily" },
//                new Period(){ Name = "Weekly" },
//                new Period(){ Name = "Monthly" },
//            };

//            await context.Periods.AddRangeAsync(periodsTestSet);
//            await context.SaveChangesAsync();
//        }
//    }

//    private static async Task SeedStatusedAsync(RaportsDBContext context)
//    {
//        if (!await context.RequestStatuses.AnyAsync())
//        {
//            IEnumerable<Status> statusesTestSet = new List<Status>()
//            {
//                new Status(){ Name = "Suspended", Description = "Wait for user interaction" },
//                new Status(){ Name = "Pending", Description = "This request is waiting for it's computation" },
//                new Status(){ Name = "Processing", Description = "This request is being computated" },
//                new Status(){ Name = "Completed", Description = "Raport was created successfully" },
//                new Status(){ Name = "Failed", Description = "Data for this report is not sufficient" },
//                new Status(){ Name = "Deleted", Description = "This request was deleted" },
//            };

//            await context.RequestStatuses.AddRangeAsync(statusesTestSet);
//            await context.SaveChangesAsync();
//        }
//    }

//    private static async Task SeedRequestsAsync(RaportsDBContext context)
//    {
//        if (!await context.Requests.AnyAsync())
//        {
//            IEnumerable<Request> requestTestSet = new List<Request>()
//            {
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1), StatusID = 1, PeriodID = 1 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(2), StatusID = 2, PeriodID = 2 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-3), EndDate = DateTime.Now.AddDays(3), StatusID = 3, PeriodID = 1 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-4), EndDate = DateTime.Now.AddDays(3), StatusID = 2, PeriodID = 2 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-5), EndDate = DateTime.Now.AddDays(3), StatusID = 1, PeriodID = 1 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-6), EndDate = DateTime.Now.AddDays(3), StatusID = 3, PeriodID = 2 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now.AddDays(3), StatusID = 1, PeriodID = 1 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-8), EndDate = DateTime.Now.AddDays(3), StatusID = 2, PeriodID = 2 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-9), EndDate = DateTime.Now.AddDays(3), StatusID = 3, PeriodID = 1 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-10), EndDate = DateTime.Now.AddDays(3), StatusID = 2, PeriodID = 2 },
//                new Request(){ RequestCreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-11), EndDate = DateTime.Now.AddDays(3), StatusID = 2, PeriodID = 1 },
//            };

//            await context.Requests.AddRangeAsync(requestTestSet);
//            await context.SaveChangesAsync();
//        }
//    }

//    private static async Task SeedRaportsAsync(RaportsDBContext context)
//    {
//        if (!await context.Raports.AnyAsync())
//        {
//            IEnumerable<Raport> raportsTestSet = new List<Raport>()
//            {
//                //new Raport(){ CreationDate = DateTime.Now, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1), PeriodID = 1, RequestID = 1 },
//            };

//            await context.Raports.AddRangeAsync(raportsTestSet);
//            await context.SaveChangesAsync();
//        }
//    }
//}
