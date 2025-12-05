using CommonServiceLibrary.Messaging.Messages.DevicesService;

namespace Devices.Application.Devices.CreateDevice;

public class CreateDeviceHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<CreateDeviceCommand, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var definedItem = await context.Devices.Where(x => x.DeviceNumber == request.DeviceNumber).FirstOrDefaultAsync();
        if (definedItem != null)
        {
            throw new DuplicateDevice(nameof(request.DeviceNumber));
        }

        var definedLocation = await context.Locations.Where(x => x.ID == request.LocationID).FirstOrDefaultAsync();
        if (definedLocation is null)
        {
            throw new EntityNotFoundException(nameof(Location), request.LocationID);
        }

        var definedStatus = await context.Statuses.Where(x => x.ID == request.StatusID).FirstOrDefaultAsync();
        if (definedStatus is null)
        {
            throw new EntityNotFoundException(nameof(Status), request.StatusID);
        }

        var definedTimestamp = await context.Timestamps.Where(x => x.ID == request.TimestampID).FirstOrDefaultAsync();
        if (definedTimestamp is null)
        {
            throw new EntityNotFoundException(nameof(Timestamp), request.TimestampID);
        }

        var newDevice = new Device()
        {
            Name = request.Name,
            DeviceNumber = request.DeviceNumber,
            Location = definedLocation,
            Status = definedStatus,
            Timestamp = definedTimestamp,
            RegisterDate = DateTime.UtcNow
        };

        //  Add device to DB and save changes.
        await context.Devices.AddAsync(newDevice, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        newDevice = await context.Devices
            .Include(x => x.Location)
            .Include(x => x.Timestamp)
            .Include(x => x.MeasurementTypes)
            .Include(x => x.Status).FirstOrDefaultAsync(x => x.ID == newDevice.ID, cancellationToken);
        if (newDevice == null)
        {
            throw new InternalServerException();
        }

        var deviceDTO = newDevice.Adapt<DefaultDeviceDTO>();
        var messageDevice = newDevice.Adapt<DevicesMessage_DefaultDevice>();

        var message = new DeviceCreated()
        {
            Device = messageDevice,
        };

        await publisher.Publish(message, cancellationToken);
        await hubContext.Clients.All.SendAsync("DeviceCreated", deviceDTO, cancellationToken);

        var response = new GetDeviceResponse(deviceDTO);

        return response;
    }
}
