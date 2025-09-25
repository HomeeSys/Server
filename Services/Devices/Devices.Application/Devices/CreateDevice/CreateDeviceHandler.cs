using Devices.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Devices.Application.Devices.CreateDevice
{
    public class CreateDeviceHandler(DevicesDBContext context, IHubContext<DeviceHub> hubContext) : IRequestHandler<CreateDeviceCommand, CreateDeviceResponse>
    {
        public async Task<CreateDeviceResponse> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            var definedItem = await context.Devices.Where(x => x.DeviceNumber == request.Device.DeviceNumber).FirstOrDefaultAsync();
            if (definedItem != null)
            {
                throw new DuplicateDevice(nameof(request.Device.DeviceNumber));
            }

            Device? device = request.Device.Adapt<Device>();
            device.RegisterDate = DateTime.Now;
            device.Name = "New Device";
            device.LocationId = 1;
            device.StatusId = 1;
            device.TimestampConfigurationId = 1;
            device.MeasurementConfigId = 1;

            //  Add device to DB and save changes.
            await context.Devices.AddAsync(device, cancellationToken);
            await context.SaveChangesAsync();

            //  Now we have to retrieve all data from such a device, including location, status, timestamp...
            device = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.Status).FirstOrDefaultAsync(x => x.DeviceNumber == request.Device.DeviceNumber, cancellationToken);
            if (device == null)
            {
                throw new InternalServerException();
            }

            await hubContext.Clients.All.SendAsync("DeviceCreated", device, cancellationToken);

            var response = new CreateDeviceResponse(device);

            return response;
        }
    }
}
