namespace Raports.Application.Handlers.Read;

public record ReadRaportStatusesCommand(int ID) : IRequest<ReadRaportStatusesResponse>;
public record ReadRaportStatusesResponse(DefaultRequestStatusDTO RequestStatusDTO);
public class ReadRaportStatusesCommandValidator : AbstractValidator<ReadRaportStatusesCommand>
{
    public ReadRaportStatusesCommandValidator() { }
}

public record ReadAllRaportStatusesCommand() : IRequest<ReadAllRaportStatusesResponse>;
public record ReadAllRaportStatusesResponse(IEnumerable<DefaultRequestStatusDTO> RequestStatusesDTOs);
public class ReadAllRaportStatusesCommandValidator : AbstractValidator<ReadAllRaportStatusesCommand>
{
    public ReadAllRaportStatusesCommandValidator() { }
}