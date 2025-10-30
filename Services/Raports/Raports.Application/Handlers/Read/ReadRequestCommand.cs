namespace Raports.Application.Handlers.Read;

public record ReadRequestCommand(int ID) : IRequest<ReadRequestResponse>;
public record ReadRequestResponse(DefaultRequestDTO RequestDTO);
public class ReadRequestCommandValidator : AbstractValidator<ReadRequestCommand>
{
    public ReadRequestCommandValidator() { }
}

public record ReadAllRequestCommand() : IRequest<ReadAllRequestResponse>;
public record ReadAllRequestResponse(IEnumerable<DefaultRequestDTO> RequestsDTOs);
public class ReadAllRequestCommandValidator : AbstractValidator<ReadAllRequestCommand>
{
    public ReadAllRequestCommandValidator() { }
}