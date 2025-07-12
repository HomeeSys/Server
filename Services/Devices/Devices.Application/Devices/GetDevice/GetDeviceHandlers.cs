namespace Devices.Application.Devices.GetDevice
{
    public class GetDeviceByIDHandler(DevicesDBContext context) : IRequestHandler<GetDeviceByIDCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(GetDeviceByIDCommand request, CancellationToken cancellationToken)
        {
            var result = await context.Devices.Include(x=>x.Location).Include(x => x.TimestampConfiguration).Include(x => x.Status).FirstOrDefaultAsync(x=>x.Id == request.ID, cancellationToken);
            if (result == null)
            {
                throw new DeviceNotFoundException(request.ID);
            }

            var response = new GetDeviceResponse(result);

            return response;
        }
    }

    public class GetDeviceByDeviceNumberHandler(DevicesDBContext context) : IRequestHandler<GetDeviceByDeviceNumberCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(GetDeviceByDeviceNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await context.Devices.Include(x => x.Location).Include(x => x.TimestampConfiguration).Include(x => x.Status).Where(x=>x.DeviceNumber == request.DeviceNumber).FirstOrDefaultAsync(cancellationToken);
            if (result == null)
            {
                throw new DeviceNotFoundException(request.DeviceNumber);
            }

            var response = new GetDeviceResponse(result);

            return response;
        }
    }

    public class GetAllDevicesHandler(DevicesDBContext context) : IRequestHandler<GetDeviceCommands, GetAllDevicesResponse>
    {
        public async Task<GetAllDevicesResponse> Handle(GetDeviceCommands request, CancellationToken cancellationToken)
        {
            var result = await context.Devices.Include(x=>x.Location).Include(x => x.TimestampConfiguration).Include(x => x.Status).Include(x=>x.TimestampConfiguration).Include(x=>x.Status).ToListAsync(cancellationToken);

            var response = new GetAllDevicesResponse(result.ToArray());

            return response;
        }
    }
}
