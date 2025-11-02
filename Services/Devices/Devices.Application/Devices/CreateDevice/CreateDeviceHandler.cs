using CommonServiceLibrary.Messaging.Events;
using Devices.Application.Hubs;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Devices.Application.Devices.CreateDevice
{
    public class CreateDeviceHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<CreateDeviceCommand, CreateDeviceResponse>
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
            device.LocationID = 1;
            device.StatusID = 1;
            device.TimestampConfigurationID = 1;

            //  Add device to DB and save changes.
            await context.Devices.AddAsync(device, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            //  We have to create Measurement config for this device.
            var measurementConfig = new MeasurementConfiguration()
            {
                Device = device,
                DeviceID = device.ID
            };

            await context.MeasurementConfigurations.AddAsync(measurementConfig, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            device.MeasurementConfigurationID = measurementConfig.ID;

            await context.SaveChangesAsync(cancellationToken);

            //  Now we have to retrieve all data from such a device, including location, status, timestamp...
            device = await context.Devices
                .Include(x => x.Location)
                .Include(x => x.TimestampConfiguration)
                .Include(x => x.MeasurementConfiguration)
                .Include(x => x.Status).FirstOrDefaultAsync(x => x.DeviceNumber == request.Device.DeviceNumber, cancellationToken);
            if (device == null)
            {
                throw new InternalServerException();
            }


            var deviceDTO = device.Adapt<DefaultDeviceDTO>();

            var mqMessage = new DeviceCreatedMessage()
            {
                NewDevice = deviceDTO,
            };
            await publisher.Publish(mqMessage, cancellationToken);

            await hubContext.Clients.All.SendAsync("DeviceCreated", deviceDTO, cancellationToken);

            var response = new CreateDeviceResponse(deviceDTO);

            return response;
        }
    }
}
