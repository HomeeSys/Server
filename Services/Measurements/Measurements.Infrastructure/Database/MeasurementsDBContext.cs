﻿namespace Measurements.Infrastructure.Database;

public class MeasurementsDBContext
{
    private readonly IConfiguration Config;
    private readonly CosmosAPI.CosmosClient Client;
    private readonly CosmosAPI.Database Database;
    private readonly CosmosAPI.Container DBContainer;

    public MeasurementsDBContext(IConfiguration configuration)
    {
        Config = configuration;

        AzureKeyCredential credential = new AzureKeyCredential(key: Config.GetValue<string>("CosmosDB:Key")!);
        Client = new CosmosAPI.CosmosClient(Config.GetValue<string>("CosmosDB:Endpoint"), credential);
        Database = Client.GetDatabase(id: Config.GetValue<string>("CosmosDB:Database"));
        DBContainer = Database.GetContainer(id: Config.GetValue<string>("CosmosDB:Container"));
    }

    public async Task<MeasurementSet> CreateMeasurement(MeasurementSet measurement)
    {
        CosmosAPI.PartitionKey partitionKey = new CosmosAPI.PartitionKey(measurement.Id.ToString());
        CosmosAPI.ItemResponse<MeasurementSet> response = await DBContainer.CreateItemAsync(measurement, partitionKey);
        if(response.StatusCode != HttpStatusCode.Created)
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
            if(exception.StatusCode == HttpStatusCode.NotFound)
            {
                throw new MeasurementDoesntExistException(ID);
            }
        }
        
        if(response == default)
        {
            throw new InternalServerException();
        }

        return response.Resource;
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
        if(response.StatusCode != HttpStatusCode.NoContent)
        {
            throw new FailedToCreateMeasurementException();
        }

        return true;
    }

    public async Task<bool> DeleteAllMeasurementSetsFromDevice(Guid DeviceNumber)
    {
        IEnumerable<MeasurementSet> measurementsToDelete = await GetMeasurements(DeviceNumber);
        foreach(MeasurementSet measurementSet in measurementsToDelete)
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
