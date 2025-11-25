namespace Measurements.Application.Consumers;

internal class CreateMeasurementConsumer(ILogger<CreateMeasurementConsumer> logger, Container database, IPublishEndpoint massTransitPublisher, IHubContext<MeasurementHub> signalRHub) : IConsumer<CreateMeasurement>
{
    public async Task Consume(ConsumeContext<CreateMeasurement> context)
    {
        var createMeasurementDTO = context.Message.Measurement;

        logger.LogInformation($"{createMeasurementDTO}");

        var response = await database.CreateItemAsync(createMeasurementDTO);
        if (response is null)
        {
            throw new Exception();
        }

        var measurementDto = response.Resource.Adapt<DefaultMeasurementDTO>();
        var messageMeasurement = response.Resource.Adapt<MeasurementsMessage_DefaultMeasurement>();
        var message = new MeasurementCreated()
        {
            Measurement = messageMeasurement,
        };

        await massTransitPublisher.Publish(message);
        await signalRHub.Clients.All.SendAsync("MeasurementCreated", measurementDto);
    }
}