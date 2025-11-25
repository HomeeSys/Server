namespace Raports.Application.Handlers.Read;

public record ReadRaportCommand(int ID) : IRequest<ReadRaportResponse>;
public record ReadRaportResponse(DefaultRaportDTO RaportDTO);
public class ReadRaportCommandValidator : AbstractValidator<ReadRaportCommand>
{
    public ReadRaportCommandValidator() { }
}

public record ReadAllRaportsCommand() : IRequest<ReadAllRaportsResponse>;
public record ReadAllRaportsResponse(IEnumerable<DefaultRaportDTO> RaportDTOs);
public class ReadAllRaportsCommandValidator : AbstractValidator<ReadAllRaportsCommand>
{
    public ReadAllRaportsCommandValidator() { }
}

public record ReadAllRaportsQueryCommand(DateTime? CreationDateFrom, DateTime? CreationDateTo, string? SortOrder, string? PeriodName, string? StatusName, int Page, int PageSize) : IRequest<ReadAllRaportsQueryResponse>;
public record ReadAllRaportsQueryResponse(PaginatedList<DefaultRaportDTO> PaginatedList);
public class ReadAllRaportsQueryCommandValidator : AbstractValidator<ReadAllRaportsQueryCommand>
{
    public ReadAllRaportsQueryCommandValidator()
    {

    }
}