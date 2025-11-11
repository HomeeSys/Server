namespace Emulators.Application.Consumers;

internal class DeviceDeletedConsumer(ILogger<DeviceDeletedConsumer> logger, EmulatorsDBContext database, ISchedulerFactory schedulerFactory, IMemoryCache cashe) : IConsumer<DeviceDeleted>
{
    public async Task Consume(ConsumeContext<DeviceDeleted> context)
    {
        var recievedDevice = context.Message.Device;
        logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Device to be deleted: '{recievedDevice.Name}' '{recievedDevice.DeviceNumber}'");

        //  Get `Device` from database if exists, and delete it.
        var serviceDbDevice = await database.Devices.FirstOrDefaultAsync(x => x.DeviceNumber == recievedDevice.DeviceNumber);
        if (serviceDbDevice is null)
        {
            logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Device with this 'DeviceNumber' -> '{recievedDevice.DeviceNumber}' was not found in this service database");
        }
        else
        {
            database.Devices.Remove(serviceDbDevice);
            logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Device removed from database: '{serviceDbDevice.DeviceNumber}'");
        }

        //  Get `Device` from cashe if exists, and delete it.
        var cashedDevice = cashe.Get<Device>($"{nameof(Device)}:{recievedDevice.DeviceNumber}");
        if (cashedDevice is null)
        {
            logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Device with this 'DeviceNumber' -> '{recievedDevice.DeviceNumber}' was not found in cashe");
        }
        else
        {
            cashe.Remove($"{nameof(Device)}:{recievedDevice.DeviceNumber}");
            logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Device removed from cashe: '{serviceDbDevice.DeviceNumber}'");
        }

        // Schedule device
        var scheduler = await schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(name: recievedDevice.DeviceNumber.ToString());
        var existingJobTrigger = await scheduler.GetTrigger(triggerKey);

        if (existingJobTrigger is null)
        {
            logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Trigger '{recievedDevice.DeviceNumber}' not found.");
        }
        else
        {
            logger.LogInformation($"{nameof(DeviceDeletedConsumer)} - Trigger '{recievedDevice.DeviceNumber}' removed.");
            await scheduler.UnscheduleJob(triggerKey);
        }
    }
}