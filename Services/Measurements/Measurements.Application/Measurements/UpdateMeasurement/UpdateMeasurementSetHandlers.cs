namespace Measurements.Application.Measurements.UpdateMeasurement;

public class UpdateMeasurementSetHandler(MeasurementsDBContext context) : IRequestHandler<UpdateMeasurementSetCommand, GetMeasurementSetResponse>
{
    public async Task<GetMeasurementSetResponse> Handle(UpdateMeasurementSetCommand request, CancellationToken cancellationToken)
    {
        MeasurementSet set = request.Measurement.Adapt<MeasurementSet>();

        var result = await context.UpdateMeasurement(set);

        var response = new GetMeasurementSetResponse(result);

        return response;
    }
}