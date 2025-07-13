namespace Measurements.Application.Measurements.CreateMeasurement;

public record CreateMeasurementCommand(CreateMeasurementSetDTO Measurement) : IRequest<GetMeasurementSetResponse>;
public class CreateMeasurementCommandValidator : AbstractValidator<CreateMeasurementCommand>
{
    public CreateMeasurementCommandValidator()
    {
        RuleFor(x => x.Measurement.DeviceNumber).NotNull().NotEmpty();
        RuleFor(x => x.Measurement.RegisterDate).NotNull().NotEmpty();
    }
}