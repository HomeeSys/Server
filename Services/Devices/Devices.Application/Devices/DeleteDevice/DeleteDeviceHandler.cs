namespace Devices.Application.Devices.DeleteDevice;

public class DeleteDeviceHandler(DevicesDBContext context, IPublishEndpoint publisher, IHubContext<DeviceHub> hubContext) : IRequestHandler<DeleteDeviceCommand, GetDeviceResponse>
{
    public async Task<GetDeviceResponse> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var definedDevice = await context.Devices
            .Include(x => x.Location)
            .Include(x => x.Timestamp)
            .Include(x => x.MeasurementTypes)
            .Include(x => x.Status)
            .Where(x => x.ID == request.DeviceID).FirstOrDefaultAsync();
        if (definedDevice == null)
        {
            throw new DeviceNotFoundException(request.DeviceID);
        }

        context.Devices.Remove(definedDevice);
        await context.SaveChangesAsync();

        var dto = definedDevice.Adapt<DefaultDeviceDTO>();
        var messageDevice = definedDevice.Adapt<DevicesMessage_DefaultDevice>();

        var mqMessage = new DeviceDeleted()
        {
            Device = messageDevice,
        };

        await publisher.Publish(mqMessage, cancellationToken);
        await hubContext.Clients.All.SendAsync("DeviceDeleted", dto, cancellationToken);

        var response = new GetDeviceResponse(dto);

        return response;
    }
}
