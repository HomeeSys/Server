namespace Raports.Application.Handlers.Read;

public record ReadAllMeasurementsResponse(IEnumerable<DefaultMeasurementDTO> MeasurementDTOs);
public record ReadAllMeasurementsCommand() : IRequest<ReadAllMeasurementsResponse>;
public class ReadAllMeasurementsCommandValidator : AbstractValidator<ReadAllMeasurementsCommand>
{
    public ReadAllMeasurementsCommandValidator() { }
}
