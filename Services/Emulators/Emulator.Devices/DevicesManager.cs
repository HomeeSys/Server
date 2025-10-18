using Emulator.Devices.Jobs;

namespace Emulator.Devices;

internal class DevicesManager
{
    private HttpClient _devicesServiceHttpClient;
    private HttpClient _measurementsServiceHttpClient;
    private bool _devicesInitialized;
    private IScheduler _scheduler;
    private List<DeviceModel> _devices;

    public DevicesManager()
    {
        _devices = new List<DeviceModel>();

        _devicesServiceHttpClient = new HttpClient();
        _devicesServiceHttpClient.BaseAddress = new Uri(@"http://devices.api:8080");

        _measurementsServiceHttpClient = new HttpClient();
        _measurementsServiceHttpClient.BaseAddress = new Uri(@"http://measurements.api:8080");
    }

    public async Task InitializeAsync()
    {
        _scheduler = await StdSchedulerFactory.GetDefaultScheduler();
    }

    public async Task WaitForMicroservicesAsync()
    {
        bool connected = false;

        do
        {
            try
            {
                var measurementsResp = await _measurementsServiceHttpClient.GetAsync("/health");
                var devicesResp = await _devicesServiceHttpClient.GetAsync("/health");
                if (devicesResp.IsSuccessStatusCode == true && measurementsResp.IsSuccessStatusCode == true)
                {
                    connected = true;
                }
            }
            catch (Exception)
            {
                connected = false;
            }
        }
        while (connected == false);

        await _scheduler.Start();
    }

    /// <summary>
    /// Handles the case when there is new device retrieved from database.
    /// If there is device with same device number that is scheduler, it will be deleted.
    /// Based on it's initial status it will be either added to schedule or no.
    /// </summary>
    /// <param name="dbDevice"></param>
    /// <returns></returns>
    public async Task DeviceAdded_HandlerAsync(DeviceModel dbDevice)
    {
        var alreadyPresent = _devices.FirstOrDefault(x => x.DeviceNumber == dbDevice.DeviceNumber);
        if (alreadyPresent != null)
        {
            _devices.Remove(alreadyPresent);
        }

        var jobKey = new JobKey(name: dbDevice.DeviceNumber.ToString());
        var triggerKey = new TriggerKey(name: dbDevice.DeviceNumber.ToString());

        var exists = _scheduler.CheckExists(jobKey);
        if (exists.Result == true)
        {
            //  Shut it down!
            var deleted = _scheduler.DeleteJob(jobKey);

            if (deleted.Result == false)
            {
                //  ...?
            }
        }

        _devices.Add(dbDevice);

        if (dbDevice.Status.Type == "Offline" || dbDevice.Status.Type == "Disabled" || dbDevice.CanGenerateMeasurementSet() == false)
        {
            //  If device is offline or disabled it will not be added to scheduler.
            return;
        }

        var jobDataMap = new JobDataMap
        {
            new(nameof(DeviceModel), dbDevice),
            new(nameof(HttpClient), _measurementsServiceHttpClient),
            new("Endpoint", "/measurements")
        };

        var jobDetails = JobBuilder.Create<SendMeasurementSetJob>()
            .WithIdentity(jobKey)
            .UsingJobData(jobDataMap)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .WithCronSchedule(dbDevice.Timestamp.CRON)
            .Build();

        await _scheduler.ScheduleJob(jobDetails, trigger);

        await Task.CompletedTask;
    }

    public async Task DeviceDeleted_HandlerAsync(DeviceModel dbDevice)
    {
        var alreadyPresent = _devices.FirstOrDefault(x => x.DeviceNumber == dbDevice.DeviceNumber);
        if (alreadyPresent == null)
        {
            _devices.Remove(alreadyPresent);
        }

        var jobKey = new JobKey(name: dbDevice.DeviceNumber.ToString());
        var triggerKey = new TriggerKey(name: dbDevice.DeviceNumber.ToString());

        var exists = _scheduler.CheckExists(jobKey);
        if (exists.Result == true)
        {
            //  Shut it down!
            var deleted = _scheduler.DeleteJob(jobKey);

            if (deleted.Result == false)
            {
                //  ...?
            }
        }
    }

    /// <summary>
    /// Handles the case when device was updated in database.
    /// Updated local devices storage and schedules device.
    /// </summary>
    /// <param name="dbDevice"></param>
    /// <returns></returns>
    public async Task DeviceUpdated_HandlerAsync(DeviceModel dbDevice)
    {
        await DeviceAdded_HandlerAsync(dbDevice);
    }

    public async Task DeviceStatusChange_HandlerAsync(DeviceModel dbDevice)
    {
        await DeviceAdded_HandlerAsync(dbDevice);
    }

    public async Task DeviceAddedAll_HandlerAsync(IEnumerable<DeviceModel> dbDevices)
    {
        foreach (var dbDevice in dbDevices)
        {
            await DeviceAdded_HandlerAsync(dbDevice);
        }
    }

    private async Task<IEnumerable<DeviceModel>> GetAllDevicesAsync()
    {
        try
        {
            HttpResponseMessage response = await _devicesServiceHttpClient.GetAsync("/devices/all");

            if (response.IsSuccessStatusCode == false)
            {
                return null;
            }

            string content = await response.Content.ReadAsStringAsync();

            var devicesDTO = JsonConvert.DeserializeObject<IEnumerable<DefaultDeviceDTO>>(content);
            if (devicesDTO == null)
            {
                return null;
            }

            var devicesModels = devicesDTO.Adapt<IEnumerable<DeviceModel>>();

            return devicesModels;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Initializes emulator. If data from database is not recieved it will stall here.
    /// </summary>
    /// <returns></returns>
    public async Task RefreshDevicesAsync()
    {
        IEnumerable<DeviceModel> dbDevices = default;

        do
        {
            try
            {
                dbDevices = await GetAllDevicesAsync();
            }
            catch (Exception) { }
        }
        while (dbDevices == null);

        await DeviceAddedAll_HandlerAsync(dbDevices);
    }
}