namespace Raports.Application.Handlers.Read;

public record ReadAllPeriodResponse(IEnumerable<DefaultPeriodDTO> PeriodsDTO);
public record ReadAllPeriodCommand() : IRequest<ReadAllPeriodResponse>;
public class ReadAllPeriodCommandValidator : AbstractValidator<ReadRaportCommand>
{
    public ReadAllPeriodCommandValidator() { }
}
