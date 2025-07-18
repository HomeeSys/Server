namespace CommonServiceLibrary.GRPC.Client;

public class MeasurementsClientGRPC
{
    private readonly GrpcChannel _channel;
    private readonly IConfiguration _configuration;
    private readonly MeasurementService.MeasurementServiceClient _measurementsServiceClient;

    public MeasurementsClientGRPC(IConfiguration configuration)
    {
        _configuration = configuration;
        _channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GRPC:Measurements")!);
        _measurementsServiceClient = new MeasurementService.MeasurementServiceClient(_channel);

        TypeAdapterConfig<MeasurementSetModel, MeasurementSetGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.CO, src => src.Co)
            .Map(dest => dest.CO2, src => src.Co2)
            .Map(dest => dest.PartuculateMatter1, src => src.ParticulateMatter1)
            .Map(dest => dest.PartuculateMatter2v5, src => src.ParticulateMatter2V5)
            .Map(dest => dest.PartuculateMatter10, src => src.ParticulateMatter10)
            .Map(dest => dest.VOC, src => src.Voc);

        TypeAdapterConfig<MeasurementModel, MeasurementGRPC>
            .NewConfig()
            .Map(dest => dest.Value, src => src.Value)
            .Map(dest => dest.Unit, src => src.Unit);
    }

    /// <summary>
    /// CosmosDB querry for MeasurementSet with given ID.
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public async Task<MeasurementSetGRPC> GetMeasurementSetByID(Guid ID)
    {
        MeasurementSetModel? model = await _measurementsServiceClient.MeasurementSetByIDAsync(new MeasurementSetRequest() { Id = ID.ToString() });

        MeasurementSetGRPC modelGrpc = model.Adapt<MeasurementSetGRPC>();

        return modelGrpc;
    }

    public async Task<IEnumerable<MeasurementSetGRPC>> GetAllMeasurementsFromDay(DateTime day)
    {
        string date = day.ToString("yyyy-MM-dd HH:mm:ss");

        Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory = () =>
            _measurementsServiceClient.MeasurementAllSetsByDay(new MeasurementSetByDateRequest() { Date = date });

        IEnumerable<MeasurementSetGRPC> modelsGrpc = await GetMultipleMeasurementSets(callFactory);

        return modelsGrpc;
    }

    public async Task<IEnumerable<MeasurementSetGRPC>> GetAllMeasurementsFromWeek(DateTime week)
    {
        string date = week.ToString("yyyy-MM-dd HH:mm:ss");

        Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory = () =>
            _measurementsServiceClient.MeasurementAllSetsByWeek(new MeasurementSetByDateRequest() { Date = date });

        IEnumerable<MeasurementSetGRPC> modelsGrpc = await GetMultipleMeasurementSets(callFactory);

        return modelsGrpc;
    }

    public async Task<IEnumerable<MeasurementSetGRPC>> GetAllMeasurementsFromMonth(DateTime month)
    {
        string date = month.ToString("yyyy-MM-dd HH:mm:ss");

        Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory = () =>
            _measurementsServiceClient.MeasurementAllSetsByMonth(new MeasurementSetByDateRequest() { Date = date });

        IEnumerable<MeasurementSetGRPC> modelsGrpc = await GetMultipleMeasurementSets(callFactory);

        return modelsGrpc;
    }

    public async Task<IEnumerable<MeasurementSetGRPC>> GetAllMeasurementsFromDay(Guid DeviceNumber, DateTime day)
    {
        string date = day.ToString("yyyy-MM-dd HH:mm:ss");
        string deviceNumber = DeviceNumber.ToString();

        Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory = () =>
            _measurementsServiceClient.MeasurementSetsByDay(new MeasurementSetFromDeviceByDateRequest() { Date = date, DeviceNumber = deviceNumber });

        IEnumerable<MeasurementSetGRPC> modelsGrpc = await GetMultipleMeasurementSets(callFactory);

        return modelsGrpc;
    }

    public async Task<IEnumerable<MeasurementSetGRPC>> GetAllMeasurementsFromWeek(Guid DeviceNumber, DateTime week)
    {
        string date = week.ToString("yyyy-MM-dd HH:mm:ss");
        string deviceNumber = DeviceNumber.ToString();

        Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory = () =>
            _measurementsServiceClient.MeasurementSetsByWeek(new MeasurementSetFromDeviceByDateRequest() { Date = date, DeviceNumber = deviceNumber });

        IEnumerable<MeasurementSetGRPC> modelsGrpc = await GetMultipleMeasurementSets(callFactory);

        return modelsGrpc;
    }

    public async Task<IEnumerable<MeasurementSetGRPC>> GetAllMeasurementsFromMonth(Guid DeviceNumber, DateTime month)
    {
        string date = month.ToString("yyyy-MM-dd HH:mm:ss");
        string deviceNumber = DeviceNumber.ToString();

        Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory = () =>
            _measurementsServiceClient.MeasurementSetsByMonth(new MeasurementSetFromDeviceByDateRequest() { Date = date, DeviceNumber = deviceNumber });

        IEnumerable<MeasurementSetGRPC> modelsGrpc = await GetMultipleMeasurementSets(callFactory);

        return modelsGrpc;
    }

    private async Task<IEnumerable<MeasurementSetGRPC>> GetMultipleMeasurementSets(Func<AsyncServerStreamingCall<MeasurementSetModel>> callFactory)
    {
        List<MeasurementSetModel> models = new List<MeasurementSetModel>();

        using AsyncServerStreamingCall<MeasurementSetModel> call = callFactory();

        while (await call.ResponseStream.MoveNext())
        {
            MeasurementSetModel current = call.ResponseStream.Current;
            models.Add(current);
        }

        IEnumerable<MeasurementSetGRPC> modelsGrpc = models.Adapt<IEnumerable<MeasurementSetGRPC>>();

        return modelsGrpc;
    }
}
