namespace Measurements.Application.Measurements.CreateMeasurement;

public class CreateMeasurementHandler(MeasurementsDBContext context, IHubContext<MeasurementHub> hubContext) : IRequestHandler<CreateMeasurementCommand, GetMeasurementSetResponse>
{
    public async Task<GetMeasurementSetResponse> Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
    {
        MeasurementSet measurement = request.Measurement.Adapt<MeasurementSet>();

        MeasurementSet response = await context.CreateMeasurement(measurement);

        var dto = response.Adapt<MeasurementSetDTO>();

        await hubContext.Clients.All.SendAsync("MeasurementCreated", dto, cancellationToken);

        return new GetMeasurementSetResponse(dto);
    }
}
