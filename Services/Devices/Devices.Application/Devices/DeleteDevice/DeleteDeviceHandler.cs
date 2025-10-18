using CommonServiceLibrary.Messaging.Events;
using Devices.Application.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Devices.Application.Devices.DeleteDevice
{
    public class DeleteDeviceHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<DeleteDeviceCommand, GetDeviceResponse>
    {
        public async Task<GetDeviceResponse> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            Device? definedDevice = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.MeasurementConfiguration)
                .Include(x => x.Status)
                .Where(x => x.DeviceNumber == request.DeviceNumber).FirstOrDefaultAsync();
            if (definedDevice == null)
            {
                throw new DeviceNotFoundException(request.DeviceNumber);
            }

            context.Devices.Remove(definedDevice);
            await context.SaveChangesAsync();

            var dto = definedDevice.Adapt<DefaultDeviceDTO>();

            var mqMessage = new DeviceDeletedMessage()
            {
                DeletedDevice = dto,
            };
            await publisher.Publish(mqMessage, cancellationToken);

            await hubContext.Clients.All.SendAsync("DeviceDeleted", dto, cancellationToken);

            var response = new GetDeviceResponse(dto);

            return response;
        }
    }
}
