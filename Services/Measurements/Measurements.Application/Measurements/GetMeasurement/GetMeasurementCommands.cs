namespace Measurements.Application.Measurements.GetMeasurement;

#region Get all measurements
public record GetMeasurementSetsCommand() : IRequest<GetAllMeasurementSetsResponse>;
public record GetAllMeasurementSetsFromDeviceByDayCommand(Guid DeviceNumber, DateTime Day) : IRequest<GetAllMeasurementSetsResponse>;
public record GetAllMeasurementSetsFromDeviceByWeekCommand(Guid DeviceNumber, DateTime Week) : IRequest<GetAllMeasurementSetsResponse>;
public record GetAllMeasurementSetsFromDeviceByMonthCommand(Guid DeviceNumber, DateTime Month) : IRequest<GetAllMeasurementSetsResponse>;
public record GetAllMeasurementSetsResponse(IEnumerable<MeasurementSet> Measurements);
public class GetMeasurementSetsCommandValidator : AbstractValidator<GetMeasurementSetsCommand>
{
    public GetMeasurementSetsCommandValidator()
    {

    }
}
#endregion

#region By Device
public record GetMeasurementSetsFromDeviceCommand(Guid DeviceNumber) : IRequest<GetAllMeasurementSetsResponse>;
public class GetMeasurementSetsFromDeviceCommandValidator : AbstractValidator<GetMeasurementSetsFromDeviceCommand>
{
    public GetMeasurementSetsFromDeviceCommandValidator()
    {

    }
}
#endregion

#region By ID
public record GetMeasurementSetCommand(Guid ID) : IRequest<GetMeasurementSetResponse>;
public record GetMeasurementSetResponse(MeasurementSetDTO MeasurementSetDTO);
#endregion

#region Get measurement
#endregion