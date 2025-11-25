namespace Devices.GRPCServer.Services;

public class DevicesServerService(ILogger<DevicesServerService> logger, DevicesDBContext dbcontext) : DevicesService.DevicesServiceBase
{
    public override async Task<GrpcLocationModel> GetLocationByName(LocationByNameRequest request, ServerCallContext context)
    {
        var locationDB = await dbcontext.Locations.FirstOrDefaultAsync(x => x.Name == request.Name);
        if (locationDB is null)
        {
            throw new Exception();
        }

        var deviceGRPC = locationDB.Adapt<GrpcLocationModel>();

        return deviceGRPC;
    }

    public override async Task GetAllLocations(LocationAllRequest request, IServerStreamWriter<GrpcLocationModel> responseStream, ServerCallContext context)
    {
        var locations = await dbcontext.Locations.ToListAsync();

        var locationsGrpc = locations.Adapt<IEnumerable<GrpcLocationModel>>();

        foreach (var device in locationsGrpc)
        {
            await responseStream.WriteAsync(device);
        }
    }

    public override async Task GetAllDevices(DeviceAllRequest request, IServerStreamWriter<GrpcDeviceModel> responseStream, ServerCallContext context)
    {
        var devicesDB = await dbcontext.Devices
            .Include(x => x.Location)
            .Include(x => x.Timestamp)
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
            .Include(x => x.Timestamp)
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