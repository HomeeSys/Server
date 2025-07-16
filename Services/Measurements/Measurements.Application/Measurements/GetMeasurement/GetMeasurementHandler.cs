namespace Measurements.Application.Measurements.GetMeasurement;

public class GetAllMeasurementsHandler(MeasurementsDBWrapper context) : IRequestHandler<GetMeasurementSetsCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetMeasurementSetsCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurements();

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementSetsFromDeviceByDayHandler(MeasurementsDBWrapper context) : IRequestHandler<GetAllMeasurementSetsFromDeviceByDayCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetAllMeasurementSetsFromDeviceByDayCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurementsFromDay(request.DeviceNumber, request.Day);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementSetsFromDeviceByWeekHandler(MeasurementsDBWrapper context) : IRequestHandler<GetAllMeasurementSetsFromDeviceByWeekCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetAllMeasurementSetsFromDeviceByWeekCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurementsFromWeek(request.DeviceNumber, request.Week);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementSetsFromDeviceByMonthHandler(MeasurementsDBWrapper context) : IRequestHandler<GetAllMeasurementSetsFromDeviceByMonthCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetAllMeasurementSetsFromDeviceByMonthCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurementsFromMonth(request.DeviceNumber, request.Month);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetAllMeasurementsFromDeviceHandler(MeasurementsDBWrapper context) : IRequestHandler<GetMeasurementSetsFromDeviceCommand, GetAllMeasurementSetsResponse>
{
    public async Task<GetAllMeasurementSetsResponse> Handle(GetMeasurementSetsFromDeviceCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurements(request.DeviceNumber);

        var response = new GetAllMeasurementSetsResponse(result);

        return response;
    }
}

public class GetMeasurementByIDHandler(MeasurementsDBWrapper context) : IRequestHandler<GetMeasurementSetCommand, GetMeasurementSetResponse>
{
    public async Task<GetMeasurementSetResponse> Handle(GetMeasurementSetCommand request, CancellationToken cancellationToken)
    {
        var result = await context.GetMeasurement(request.ID);

        var response = new GetMeasurementSetResponse(result);

        return response;
    }
}