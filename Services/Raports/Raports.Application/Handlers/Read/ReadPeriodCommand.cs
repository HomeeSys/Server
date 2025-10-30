namespace Raports.Application.Handlers.Read;

public record ReadPeriodCommand(int ID) : IRequest<ReadPeriodResponse>;
public record ReadPeriodResponse(DefaultPeriodDTO PeriodDTO);
public class ReadPeriodCommandValidator : AbstractValidator<ReadPeriodCommand>
{
    public ReadPeriodCommandValidator() { }
}

public record ReadAllPeriodCommand() : IRequest<ReadAllPeriodResponse>;
public record ReadAllPeriodResponse(IEnumerable<DefaultPeriodDTO> PeriodsDTOs);
public class ReadAllPeriodCommandValidator : AbstractValidator<ReadAllPeriodCommand>
{
    public ReadAllPeriodCommandValidator() { }
}