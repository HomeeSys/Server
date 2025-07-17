namespace Measurements.Application.Measurements.DeleteMeasurement;

public class DeleteMeasurementSetHandler(MeasurementsDBContext context) : IRequestHandler<DeleteMeasurementSetCommand, DeleteMeasurementSetResponse>
{
    public async Task<DeleteMeasurementSetResponse> Handle(DeleteMeasurementSetCommand request, CancellationToken cancellationToken)
    {
        var result = await context.DeleteMeasurementSet(request.ID);

        var response = new DeleteMeasurementSetResponse(result);

        return response;
    }
}

public class DeleteAllMeasurementFromDevicveSetHandler(MeasurementsDBContext context) : IRequestHandler<DeleteAllMeasurementSetsFromDeviceCommand, DeleteMeasurementSetResponse>
{
    public async Task<DeleteMeasurementSetResponse> Handle(DeleteAllMeasurementSetsFromDeviceCommand request, CancellationToken cancellationToken)
    {
        var result = await context.DeleteAllMeasurementSetsFromDevice(request.DeviceNumber);

        var response = new DeleteMeasurementSetResponse(result);

        return response;
    }
}