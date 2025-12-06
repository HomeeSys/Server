namespace Raports.Application.Handlers.Update;

public record RetryRaportCommand(int RaportID) : IRequest<ReadRaportResponse>;

public class RetryRaportCommandValidator : AbstractValidator<RetryRaportCommand>
{
    public RetryRaportCommandValidator()
    {
        RuleFor(x => x.RaportID).GreaterThan(0);
    }
}
