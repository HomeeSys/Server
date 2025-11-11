namespace Emulators.Application.Jobs;

internal class EnqueueMeasurementGenerationJob(ILogger<EnqueueMeasurementGenerationJob> logger, IPublishEndpoint publisher) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var jobDataArg = context.MergedJobDataMap["Device"];

            if (jobDataArg is not DefaultDeviceDTO jobDeviceArgument)
            {
                logger.LogError($"{nameof(EnqueueMeasurementGenerationJob)} - Failed to parse input arguments!");
                return;
            }

            var topicMessage = new DeviceGenerateMeasurement()
            {
                Device = jobDeviceArgument,
                Date = DateTime.UtcNow,
            };

            await publisher.Publish(topicMessage);
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(EnqueueMeasurementGenerationJob)} - '{ex.Message}'");
        }
    }
}