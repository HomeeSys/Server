namespace Measurements.Application.Consumers;

internal class CreateMeasurementConsumer(ILogger<CreateMeasurementConsumer> logger) : IConsumer<CreateMeasurement>
{
    public async Task Consume(ConsumeContext<CreateMeasurement> context)
    {
        var createMeasurementDTO = context.Message.Measurement;

        logger.LogInformation($"{createMeasurementDTO}");
    }
}