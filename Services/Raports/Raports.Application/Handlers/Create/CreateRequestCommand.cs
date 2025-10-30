namespace Raports.Application.Handlers.Create;

public record CreateRequestCommand(CreateRequestDTO CreateDTO) : IRequest<ReadRequestResponse>;
public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator() { }
}