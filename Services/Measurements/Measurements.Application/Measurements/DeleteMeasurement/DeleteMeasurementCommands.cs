namespace Measurements.Application.Measurements.DeleteMeasurement;

public record DeleteMeasurementSetCommand(Guid ID) : IRequest<DeleteMeasurementSetResponse>;
public class DeleteMeasurementSetCommandValidator : AbstractValidator<DeleteMeasurementSetCommand>
{
    public DeleteMeasurementSetCommandValidator()
    {
        RuleFor(x => x.ID).NotNull().NotEmpty();
    }
}

public record DeleteAllMeasurementSetsFromDeviceCommand(Guid DeviceNumber) : IRequest<DeleteMeasurementSetResponse>;
public class DeleteAllMeasurementSetsFromDeviceCommandValidator : AbstractValidator<DeleteAllMeasurementSetsFromDeviceCommand>
{
    public DeleteAllMeasurementSetsFromDeviceCommandValidator()
    {
        RuleFor(x => x.DeviceNumber).NotNull().NotEmpty();
    }
}

public record DeleteMeasurementSetResponse(bool Status);