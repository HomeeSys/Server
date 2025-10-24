using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Hosting;

namespace Measurements.Infrastructure.Database;

public class MeasurementsDBContext
{
    private readonly IConfiguration Config;
    private readonly IHostEnvironment Env;
    private readonly CosmosAPI.CosmosClient Client;
    private readonly CosmosAPI.Database Database;
    private CosmosAPI.Container DBContainer;

    public MeasurementsDBContext(IConfiguration configuration, IHostEnvironment environment)
    {
        Config = configuration;
        Env = environment;

        AzureKeyCredential credential = new AzureKeyCredential(key: Config.GetValue<string>("CosmosDB:Key")!);
        Client = new CosmosAPI.CosmosClient(Config.GetValue<string>("CosmosDB:Endpoint"), credential);
        Database = Client.GetDatabase(id: Config.GetValue<string>("CosmosDB:Database"));

        string containerId = Config.GetValue<string>("CosmosDB:ContainerDev");

        if (Env.IsProduction())
        {
            containerId = Config.GetValue<string>("CosmosDB:ContainerProd");
        }

        DBContainer = Database.GetContainer(containerId);
    }
    public async Task InitializeDatabase()
    {
        if (Env.IsProduction())
        {
            string containerId = Config.GetValue<string>("CosmosDB:ContainerProd");
            DBContainer = Database.GetContainer(containerId);
        }
        else
        {
            string containerId = Config.GetValue<string>("CosmosDB:ContainerDev");
            string partitionKey = Config.GetValue<string>("CosmosDB:PartitionKey") ?? "/id";
            //string partitionKey = "/id";

            DBContainer = await RecreateContainerAsync(Database, containerId, partitionKey);
        }
    }

    private static async Task<Container> RecreateContainerAsync(CosmosAPI.Database database, string containerId, string partitionKeyPath, int throughput = 400)
    {
        Container container = database.GetContainer(containerId);

        bool exists = await ContainerExistsAsync(container);
        if (exists)
        {
            await container.DeleteContainerAsync();
        }

        ContainerResponse response = await database.CreateContainerAsync(
            new ContainerProperties
            {
                Id = containerId,
                PartitionKeyPath = partitionKeyPath
            }
        );

        return response.Container;
    }

    private static async Task<bool> ContainerExistsAsync(Container container)
    {
        try
        {
            await container.ReadContainerAsync();
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    public async Task<MeasurementSet> CreateMeasurement(MeasurementSet measurement)
    {
        CosmosAPI.PartitionKey partitionKey = new CosmosAPI.PartitionKey(measurement.Id.ToString());
        CosmosAPI.ItemResponse<MeasurementSet> response = await DBContainer.CreateItemAsync(measurement, partitionKey);
        if (response.StatusCode != HttpStatusCode.Created)
        {
            throw new FailedToCreateMeasurementException();
        }

        return response.Resource;
    }

    public async Task<MeasurementSet> GetMeasurement(Guid ID)
    {
        string id = ID.ToString();
        CosmosAPI.PartitionKey partitionKey = new CosmosAPI.PartitionKey(id);
        CosmosAPI.ItemResponse<MeasurementSet> response = default;

        try
        {
            response = await DBContainer.ReadItemAsync<MeasurementSet>(id, partitionKey);
        }
        catch (CosmosException exception)
        {
            if (exception.StatusCode == HttpStatusCode.NotFound)
            {
                throw new MeasurementDoesntExistException(ID);
            }
        }

        if (response == default)
        {
            throw new InternalServerException();
        }

        return response.Resource;
    }

    public async Task<DateTime?> GetMinimalDate()
    {
        var query = DBContainer.GetItemLinqQueryable<MeasurementSet>()
                               .OrderBy(x => x.RegisterDate)
                               .Select(x => x.RegisterDate)
                               .Take(1)
                               .ToFeedIterator();

        if (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            return response.FirstOrDefault();
        }

        return null;
    }

    public async Task<DateTime?> GetMaximalDate()
    {
        var query = DBContainer.GetItemLinqQueryable<MeasurementSet>()
                               .OrderByDescending(x => x.RegisterDate)
                               .Select(x => x.RegisterDate)
                               .Take(1)
                               .ToFeedIterator();

        if (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            return response.FirstOrDefault();
        }

        return null;
    }

    public async Task<IEnumerable<MeasurementSet>> GetMeasurements()
    {
        CosmosAPI.FeedIterator<MeasurementSet> measurementIterator = DBContainer.GetItemQueryIterator<MeasurementSet>();
        List<MeasurementSet> measurements = new List<MeasurementSet>();

        while (measurementIterator.HasMoreResults)
        {
            CosmosAPI.FeedResponse<MeasurementSet> response = await measurementIterator.ReadNextAsync();

            measurements.AddRange(response.Resource);
        }

        return measurements;
    }

    /// <summary>
    /// Retrieves all measurements for given device
    /// </summary>
    /// <param name="DeviceNumber"></param>
    /// <returns></returns>
    public async Task<IEnumerable<MeasurementSet>> GetMeasurements(Guid DeviceNumber)
    {
        string noSqlQuerry = $"select * from c where c.DeviceNumber = '{DeviceNumber}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    public async Task<IEnumerable<MeasurementSet>> GetAllMeasurementsFromDay(DateTime dayDate)
    {
        DateTime from = dayDate.Date;
        DateTime to = from.AddDays(1);

        string str_from = from.ToString("yyyy-MM-dd HH:mm:ss");
        string str_to = to.ToString("yyyy-MM-dd HH:mm:ss");

        string noSqlQuerry = $"select * from c where c.RegisterDate >= '{str_from}' and c.RegisterDate < '{str_to}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    public async Task<IEnumerable<MeasurementSet>> GetMeasurementsFromDay(Guid DeviceNumber, DateTime dayDate)
    {
        DateTime from = dayDate.Date;
        DateTime to = from.AddDays(1);

        string str_from = from.ToString("yyyy-MM-dd HH:mm:ss");
        string str_to = to.ToString("yyyy-MM-dd HH:mm:ss");
        string str_deviceNumber = DeviceNumber.ToString();

        string noSqlQuerry = $"select * from c where c.RegisterDate >= '{str_from}' and c.RegisterDate < '{str_to}' and c.DeviceNumber = '{str_deviceNumber}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    public async Task<IEnumerable<MeasurementSet>> GetAllMeasurementsFromWeek(DateTime date)
    {
        DateTime to = date.Date;
        DateTime from = to.AddDays(-7);

        string str_from = from.ToString("yyyy-MM-dd HH:mm:ss");
        string str_to = to.ToString("yyyy-MM-dd HH:mm:ss");

        string noSqlQuerry = $"select * from c where c.RegisterDate >= '{str_from}' and c.RegisterDate < '{str_to}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    /// <summary>
    /// Returns collection of `MeasurementSet` that was taken last week.
    /// </summary>
    /// <param name="DeviceNumber"></param>
    /// <param name="weekDate">Last day of the week.</param>
    /// <returns></returns>
    public async Task<IEnumerable<MeasurementSet>> GetMeasurementsFromWeek(Guid DeviceNumber, DateTime weekDate)
    {
        DateTime to = weekDate.Date;
        DateTime from = to.AddDays(-7);

        string str_from = from.ToString("yyyy-MM-dd HH:mm:ss");
        string str_to = to.ToString("yyyy-MM-dd HH:mm:ss");
        string str_deviceNumber = DeviceNumber.ToString();

        string noSqlQuerry = $"select * from c where c.RegisterDate >= '{str_from}' and c.RegisterDate < '{str_to}' and c.DeviceNumber = '{str_deviceNumber}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    public async Task<IEnumerable<MeasurementSet>> GetAllMeasurementsFromMonth(DateTime date)
    {
        DateTime to = date.Date;
        DateTime from = to.AddMonths(-1);

        string str_from = from.ToString("yyyy-MM-dd HH:mm:ss");
        string str_to = to.ToString("yyyy-MM-dd HH:mm:ss");

        string noSqlQuerry = $"select * from c where c.RegisterDate >= '{str_from}' and c.RegisterDate < '{str_to}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    public async Task<IEnumerable<MeasurementSet>> GetMeasurementsFromMonth(Guid DeviceNumber, DateTime weekDate)
    {
        DateTime to = weekDate.Date;
        DateTime from = to.AddMonths(-1);

        string str_from = from.ToString("yyyy-MM-dd HH:mm:ss");
        string str_to = to.ToString("yyyy-MM-dd HH:mm:ss");
        string str_deviceNumber = DeviceNumber.ToString();

        string noSqlQuerry = $"select * from c where c.RegisterDate >= '{str_from}' and c.RegisterDate < '{str_to}' and c.DeviceNumber = '{str_deviceNumber}'";

        return await GetMeasurementSetsFromQuerry(noSqlQuerry);
    }

    public async Task<MeasurementSet> UpdateMeasurement(MeasurementSet measurement)
    {
        throw new NotImplementedException();
    }

    public async Task<IQueryable<MeasurementSet>> GetMeasurementSetsQuery(List<Guid> deviceNumbers, DateTime? DateFrom, DateTime? DateTo, string? SortOrder, int Page, int PageSize)
    {
        var query = DBContainer.GetItemLinqQueryable<MeasurementSet>(true).AsQueryable();


        if (deviceNumbers != null)
        {
            query = query.Where(x => deviceNumbers.Contains(x.DeviceNumber) == true);
        }

        if (DateFrom != null)
        {
            var fromLocal = DateTime.SpecifyKind(DateFrom.Value, DateTimeKind.Unspecified);
            query = query.Where(x => x.RegisterDate >= fromLocal);
        }

        if (DateTo != null)
        {
            var toLocal = DateTime.SpecifyKind(DateTo.Value, DateTimeKind.Unspecified);
            query = query.Where(x => x.RegisterDate <= toLocal);
        }

        string sortOrder = "asc";
        if (!string.IsNullOrEmpty(SortOrder))
        {
            sortOrder = SortOrder;
        }

        if (sortOrder == "asc")
        {
            query = query.OrderBy(x => x.RegisterDate);
        }
        else
        {
            query = query.OrderByDescending(x => x.RegisterDate);
        }

        return query;
    }

    private async Task<IEnumerable<MeasurementSet>> GetMeasurementSetsFromQuerry(string noSql)
    {
        CosmosAPI.QueryDefinition query = new CosmosAPI.QueryDefinition(noSql);

        CosmosAPI.FeedIterator<MeasurementSet> measurementIterator = DBContainer.GetItemQueryIterator<MeasurementSet>(query);
        List<MeasurementSet> measurements = new List<MeasurementSet>();

        while (measurementIterator.HasMoreResults)
        {
            CosmosAPI.FeedResponse<MeasurementSet> response = await measurementIterator.ReadNextAsync();

            measurements.AddRange(response.Resource);
        }

        return measurements;
    }

    public async Task<bool> DeleteMeasurementSet(Guid ID)
    {
        ItemResponse<MeasurementSet> response = await DBContainer.DeleteItemAsync<MeasurementSet>(ID.ToString(), new PartitionKey(ID.ToString()));
        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            throw new FailedToCreateMeasurementException();
        }

        return true;
    }

    public async Task<bool> DeleteAllMeasurementSetsFromDevice(Guid DeviceNumber)
    {
        IEnumerable<MeasurementSet> measurementsToDelete = await GetMeasurements(DeviceNumber);
        foreach (MeasurementSet measurementSet in measurementsToDelete)
        {
            ItemResponse<MeasurementSet> response = await DBContainer.DeleteItemAsync<MeasurementSet>(measurementSet.Id.ToString(), new PartitionKey(measurementSet.Id.ToString()));
            if (response == null)
            {
                throw new FailedToCreateMeasurementException();
            }
        }

        return true;
    }
}
