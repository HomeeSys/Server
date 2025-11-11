namespace Measurements.Application.Measurements.CreateMeasurement;

public class CreateMeasurementHandler(Container cosmosContainer, IPublishEndpoint massTransitPublisher, IHubContext<MeasurementHub> signalRHub) : IRequestHandler<CreateMeasurementCommand, GetMeasurementResponse>
{
    public async Task<GetMeasurementResponse> Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
    {
        var measurement = request.CreateMeasurement.Adapt<Measurement>();
        measurement.RecordedAt = DateTime.UtcNow;
        measurement.ID = Guid.NewGuid();

        var response = await cosmosContainer.CreateItemAsync(measurement);
        if (response is null)
        {
            throw new Exception();
        }

        var measurementDto = response.Resource.Adapt<DefaultMeasurementDTO>();

        var message = new MeasurementCreatedMessage()
        {
            CreatedMeasurement = measurementDto,
        };

        await massTransitPublisher.Publish(message, cancellationToken);
        await signalRHub.Clients.All.SendAsync("MeasurementCreated", measurementDto, cancellationToken);

        return new GetMeasurementResponse(measurementDto);
    }
}
