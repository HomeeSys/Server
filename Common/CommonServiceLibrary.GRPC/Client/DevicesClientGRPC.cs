namespace CommonServiceLibrary.GRPC.Client;

public class DevicesClientGRPC
{
    private readonly GrpcChannel _channel;
    private readonly IConfiguration _configuration;
    private readonly DevicesService.DevicesServiceClient _devicesServiceClient;

    public DevicesClientGRPC(IConfiguration configuration)
    {
        _configuration = configuration;
        _channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GRPC:Devices")!);
        _devicesServiceClient = new DevicesService.DevicesServiceClient(_channel);

        TypeAdapterConfig<DeviceModel, DeviceGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.LocationID, src => src.LocationId)
            .Map(dest => dest.TimestampConfigurationID, src => src.TimestampConfigurationId)
            .Map(dest => dest.StatusID, src => src.StatusId);

        TypeAdapterConfig<DeviceGRPC, DeviceModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate)
            .Map(dest => dest.LocationId, src => src.LocationID)
            .Map(dest => dest.TimestampConfigurationId, src => src.TimestampConfigurationID)
            .Map(dest => dest.StatusId, src => src.StatusID)
            .Map(dest => dest.Name, src => src.Name);

        TypeAdapterConfig<TimestampConfigurationModel, TimestampConfigurationGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id);

        TypeAdapterConfig<LocationModel, LocationGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id);

        TypeAdapterConfig<StatusModel, StatusGRPC>
            .NewConfig()
            .Map(dest => dest.ID, src => src.Id);
    }

    public async Task<DeviceGRPC> GetDeviceByDeviceNumber(Guid DeviceNumber)
    {
        DeviceModel? model = await _devicesServiceClient.GetDeviceByDeviceNumberAsync(new DeviceRequest() { DeviceNumber = DeviceNumber.ToString() });

        DeviceGRPC modelGrpc = model.Adapt<DeviceGRPC>();

        return modelGrpc;
    }

    public async Task<IEnumerable<DeviceGRPC>> GetAllDevices()
    {
        List<DeviceModel> models = new List<DeviceModel>();

        using (var call = _devicesServiceClient.GetAllDevices(new DeviceAllRequest()))
        {
            while (await call.ResponseStream.MoveNext())
            {
                DeviceModel current = call.ResponseStream.Current;
                models.Add(current);
            }
        }

        IEnumerable<DeviceGRPC> modelsGrpc = models.Adapt<IEnumerable<DeviceGRPC>>();

        return modelsGrpc;
    }
}
