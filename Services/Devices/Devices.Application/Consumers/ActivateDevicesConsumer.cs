namespace Devices.Application.Consumers;

internal class ActivateDevicesConsumer(ILogger<ActivateDevicesConsumer> logger, DevicesDBContext database, IPublishEndpoint publisher) : IConsumer<ActivateDevices>
{
    public async Task Consume(ConsumeContext<ActivateDevices> context)
    {
        logger.LogInformation("");

        var devices = await database.Devices
            .Include(x => x.Timestamp)
            .Include(x => x.Status)
            .Include(x => x.Location)
            .Include(x => x.MeasurementTypes)
            .ToListAsync();

        var devicesDtos = devices.Adapt<IEnumerable<DefaultDeviceDTO>>();

        var deviceActivatedMessages = devicesDtos.Select(x => new DeviceActivated() { Device = x.Adapt<DevicesMessage_DefaultDevice>() });

        await publisher.PublishBatch(deviceActivatedMessages);
    }
}
