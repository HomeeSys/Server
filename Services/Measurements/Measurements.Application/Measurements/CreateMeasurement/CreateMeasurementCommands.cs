namespace Measurements.Application.Measurements.CreateMeasurement;

public record CreateMeasurementCommand(CreateMeasurementDTO CreateMeasurement) : IRequest<GetMeasurementResponse>;
public class CreateMeasurementCommandValidator : AbstractValidator<CreateMeasurementCommand>
{
    public CreateMeasurementCommandValidator()
    {
        RuleFor(x => x.CreateMeasurement)
            .NotNull()
            .WithMessage("Measurement data must be provided.")
            .Must(HasAtLeastOneValue)
            .WithMessage("At least one measurement value must be provided.");
    }

    private bool HasAtLeastOneValue(CreateMeasurementDTO dto)
    {
        if (dto is null)
        {
            return false;
        }

        return new double?[]
        {
            dto.Temperature,
            dto.Humidity,
            dto.CarbonDioxide,
            dto.VolatileOrganicCompounds,
            dto.ParticulateMatter1,
            dto.ParticulateMatter2v5,
            dto.ParticulateMatter10,
            dto.Formaldehyde,
            dto.CarbonMonoxide,
            dto.Ozone,
            dto.Ammonia,
            dto.Airflow,
            dto.AirIonizationLevel,
            dto.Oxygen,
            dto.Radon,
            dto.Illuminance,
            dto.SoundLevel
        }.Any(v => v.HasValue);
    }
}