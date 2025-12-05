namespace Raports.Application.Handlers.Read;

public record ReadAllStatusesCommand() : IRequest<ReadAllStatusesResponse>;
public record ReadAllStatusesResponse(IEnumerable<DefaultStatusDTO> StatusesDTOs);
public class ReadAllStatusesCommandValidator : AbstractValidator<ReadAllStatusesCommand>
{
    public ReadAllStatusesCommandValidator() { }
}