namespace Measurements.Application.Measurements.CreateMeasurement;

public class CreateMeasurementHandler(MeasurementsDBWrapper context) : IRequestHandler<CreateMeasurementCommand, GetMeasurementSetResponse>
{
    public async Task<GetMeasurementSetResponse> Handle(CreateMeasurementCommand request, CancellationToken cancellationToken)
    {
        MeasurementSet measurement = request.Measurement.Adapt<MeasurementSet>();

        MeasurementSet response = await context.CreateMeasurement(measurement);

        return new GetMeasurementSetResponse(response);
    }
}
