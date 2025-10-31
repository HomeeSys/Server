namespace Raports.Application.Handlers.Delete;

public record SoftDeleteRequestCommand(int RequestID) : IRequest<ReadRequestResponse>;
public class SoftDeleteRequestCommandValidator : AbstractValidator<SoftDeleteRequestCommand>
{
    public SoftDeleteRequestCommandValidator() { }
}