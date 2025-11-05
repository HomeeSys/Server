namespace Devices.GRPCServer.Services;

public class DevicesServerService(ILogger<DevicesServerService> logger, DevicesDBContext dbcontext) : DevicesService.DevicesServiceBase
{
    public override async Task GetAllDevices(DeviceAllRequest request, IServerStreamWriter<GrpcDeviceModel> responseStream, ServerCallContext context)
    {
        var devicesDB = await dbcontext.Devices
            .Include(x => x.Location)
            .Include(x => x.TimestampConfiguration)
            .Include(x => x.MeasurementConfiguration)
            .Include(x => x.Status)
            .ToListAsync();

        var devicesGRPC = devicesDB.Adapt<IEnumerable<GrpcDeviceModel>>();

        foreach (var device in devicesGRPC)
        {
            await responseStream.WriteAsync(device);
        }
    }

    public override async Task<GrpcDeviceModel> GetDeviceByDeviceNumber(DeviceRequest request, ServerCallContext context)
    {
        Guid guid = Guid.Parse(request.DeviceNumber);

        var deviceDB = await dbcontext.Devices
            .Include(x => x.Location)
            .Include(x => x.TimestampConfiguration)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(x => x.DeviceNumber == guid);

        if (deviceDB == null)
        {
            throw new Exception();
        }

        var deviceGRPC = deviceDB.Adapt<GrpcDeviceModel>();

        return deviceGRPC;
    }
}

/*
  
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

 */