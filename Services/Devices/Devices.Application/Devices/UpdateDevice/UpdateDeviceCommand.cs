namespace Devices.Application.Devices.UpdateDevice;

public record UpdateDeviceCommand(int DeviceID, string? Name, int? LocationID, int? TimestampID, int? StatusID) : IRequest<GetDeviceResponse>;
public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
{
    public UpdateDeviceCommandValidator()
    {
    }
}

public record UpdateDeviceStatusCommand(int DeviceID, int StatusID) : IRequest<GetDeviceResponse>;
public class UpdateDeviceStatusCommandValidator : AbstractValidator<UpdateDeviceStatusCommand>
{
    public UpdateDeviceStatusCommandValidator()
    {
    }
}

public record UpdateDeviceMeasurementTypeCommand(int DeviceID, int[] MeasurementTypeIDs) : IRequest<GetDeviceResponse>;
public class UpdateDeviceMeasurementTypeCommandValidator : AbstractValidator<UpdateDeviceMeasurementTypeCommand>
{
    public UpdateDeviceMeasurementTypeCommandValidator()
    {
    }
}