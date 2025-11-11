namespace Emulators.Application.Consumers;

internal class DeviceStatusChangedConsumer(ILogger<DeviceStatusChangedConsumer> logger, EmulatorsDBContext database, ISchedulerFactory schedulerFactory, IMemoryCache cashe) : IConsumer<DeviceStatusChanged>
{
    public async Task Consume(ConsumeContext<DeviceStatusChanged> context)
    {
        var recievedDevice = context.Message.Device;
        logger.LogInformation($"{nameof(DeviceStatusChangedConsumer)} - Device activated: '{recievedDevice.Name}' '{recievedDevice.DeviceNumber}'");

        var cashedDevice = cashe.Get<Device>($"{nameof(Device)}:{recievedDevice.DeviceNumber}");
        if (cashedDevice is null)
        {
            var serviceDbDevice = await database.Devices.FirstOrDefaultAsync(x => x.DeviceNumber == recievedDevice.DeviceNumber);
            if (serviceDbDevice is null)
            {
                logger.LogInformation($"{nameof(DeviceStatusChangedConsumer)} - Device with this 'DeviceNumber' -> '{recievedDevice.DeviceNumber}' was not found in this service database");
                return;
            }

            cashe.Set($"{nameof(Device)}:{serviceDbDevice.DeviceNumber}", serviceDbDevice);
            logger.LogInformation($"{nameof(DeviceStatusChangedConsumer)} - Device added to cashe: '{serviceDbDevice.DeviceNumber}'");

            cashedDevice = serviceDbDevice;
        }

        //  Schedule device
        var scheduler = await schedulerFactory.GetScheduler();
        var triggerKey = new TriggerKey(name: recievedDevice.DeviceNumber.ToString());
        var jobDataMap = new JobDataMap()
        {
            { "Device", recievedDevice },
        };

        var newTrigger = TriggerBuilder.Create()
            .ForJob(nameof(EnqueueMeasurementGenerationJob))
            .WithIdentity(triggerKey)
            .UsingJobData(jobDataMap)
            .WithCronSchedule(recievedDevice.Timestamp.Cron)
            .Build();

        var existingJobTrigger = await scheduler.GetTrigger(triggerKey);
        if (existingJobTrigger is null)
        {
            logger.LogInformation($"{nameof(DeviceStatusChangedConsumer)} - Scheduling new trigger...");
            await scheduler.ScheduleJob(newTrigger);
        }
        else
        {
            logger.LogInformation($"{nameof(DeviceStatusChangedConsumer)} - Rescheduling trigger...");
            await scheduler.RescheduleJob(triggerKey, newTrigger);
        }

        //  If device is not online or it doesn't have assigned measurement types.
        if (recievedDevice.Status.Type != "Online" || recievedDevice.MeasurementTypes.Count == 0)
        {
            //  Pause every device that's status is not 'Online'
            logger.LogInformation($"{nameof(DeviceStatusChangedConsumer)} - Pausing trigger...");
            await scheduler.PauseTrigger(triggerKey);
        }
    }
}