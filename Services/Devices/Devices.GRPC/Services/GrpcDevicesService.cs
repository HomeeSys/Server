namespace Devices.GRPC.Services;

public class GrpcDevicesService : DevicesService.DevicesServiceBase
{
    private readonly ILogger<GrpcDevicesService> _logger;
    private readonly DevicesDBContext _dbContext;
    public GrpcDevicesService(ILogger<GrpcDevicesService> logger, DevicesDBContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;

        TypeAdapterConfig<Device, DeviceModel>
            .NewConfig()
            .Map(dest => dest.Id, src => src.ID)
            .Map(dest => dest.DeviceNumber, src => src.DeviceNumber)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.LocationId, src => src.LocationID)
            .Map(dest => dest.TimestampConfigurationId, src => src.TimestampConfigurationID)
            .Map(dest => dest.StatusId, src => src.StatusID)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    public override async Task<DeviceModel> GetDeviceByDeviceNumber(DeviceRequest request, ServerCallContext context)
    {
        Guid guid = Guid.Parse(request.DeviceNumber);
        Device? deviceDB = await _dbContext.Devices.Include(x => x.Location).Include(x => x.TimestampConfiguration).Include(x => x.Status).FirstOrDefaultAsync(x => x.DeviceNumber == guid);
        if (deviceDB == null)
        {
            throw new Exception();
        }

        DeviceModel deviceGRPC = deviceDB.Adapt<DeviceModel>();

        return deviceGRPC;
    }

    public override async Task GetAllDevices(DeviceAllRequest request, IServerStreamWriter<DeviceModel> responseStream, ServerCallContext context)
    {
        IEnumerable<Device> devicesDB = await _dbContext.Devices.Include(x => x.Location).Include(x => x.TimestampConfiguration).Include(x => x.Status).ToListAsync();
        IEnumerable<DeviceModel> devicesGRPC = devicesDB.Adapt<IEnumerable<DeviceModel>>();

        foreach (DeviceModel device in devicesGRPC)
        {
            await responseStream.WriteAsync(device);
        }
    }
}
