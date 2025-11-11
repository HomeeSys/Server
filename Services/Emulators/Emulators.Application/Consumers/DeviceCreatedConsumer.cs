namespace Emulators.Application.Consumers;

internal class DeviceCreatedConsumer(ILogger<DeviceCreatedConsumer> logger, EmulatorsDBContext database, ISchedulerFactory schedulerFactory, IMemoryCache cashe) : IConsumer<DeviceCreated>
{
    public async Task Consume(ConsumeContext<DeviceCreated> context)
    {
        var recievedDevice = context.Message.Device;

        logger.LogInformation($"{nameof(DeviceCreatedConsumer)} - Recieved new device data: '{context.Message.Device}'");

        var cashedDevice = cashe.Get<Device>($"{nameof(Device)}:{recievedDevice.DeviceNumber}");
        if (cashedDevice is not null)
        {
            //  Not found in cashe
            cashe.Remove(cashedDevice);
        }

        var foundDevice = await database.Devices.FirstOrDefaultAsync(x => x.DeviceNumber == recievedDevice.DeviceNumber);
        if (foundDevice is not null)
        {
            //  This device is already defined.
            logger.LogInformation($"{nameof(DeviceCreatedConsumer)} - recieved device is already defined in Emulators.DB: '{context.Message.Device}'");
        }
        else
        {
            //  Create new device
            var newDevice = new Device()
            {
                DeviceNumber = recievedDevice.DeviceNumber,
                Spread = Math.Round(new Random().NextDouble() * 12, 2)
            };

            await database.Devices.AddAsync(newDevice);
            await database.SaveChangesAsync();

            cashe.Set($"{nameof(Device)}:{recievedDevice.DeviceNumber}", foundDevice);
        }


        //  Scheduling
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
            logger.LogInformation($"{nameof(DeviceCreatedConsumer)} - Scheduling new trigger...");
            await scheduler.ScheduleJob(newTrigger);
        }
        else
        {
            logger.LogInformation($"{nameof(DeviceCreatedConsumer)} - Rescheduling trigger...");
            await scheduler.RescheduleJob(triggerKey, newTrigger);
        }

        //  If device is not online or it doesn't have assigned measurement types.
        if (recievedDevice.Status.Type != "Online" || recievedDevice.MeasurementTypes.Count == 0)
        {
            //  Pause every device that's status is not 'Online'
            logger.LogInformation($"{nameof(DeviceCreatedConsumer)} - Pausing trigger...");
            await scheduler.PauseTrigger(triggerKey);
        }
    }
}