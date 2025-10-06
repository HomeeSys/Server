using Devices.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Devices.Application.Devices.DeleteDevice
{
    public class DeleteDeviceHandler(DevicesDBContext context, IHubContext<DeviceHub> hubContext) : IRequestHandler<DeleteDeviceCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            Device? definedDevice = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.Status)
                .Where(x => x.DeviceNumber == request.DeviceNumber).FirstOrDefaultAsync();
            if (definedDevice == null)
            {
                throw new DeviceNotFoundException(request.DeviceNumber);
            }

            context.Devices.Remove(definedDevice);
            await context.SaveChangesAsync();

            var dto = definedDevice.Adapt<DefaultDeviceDTO>();

            await hubContext.Clients.All.SendAsync("DeviceDeleted", dto, cancellationToken);

            var response = new GetDeviceResponse(dto);

            return response;
        }
    }
}
