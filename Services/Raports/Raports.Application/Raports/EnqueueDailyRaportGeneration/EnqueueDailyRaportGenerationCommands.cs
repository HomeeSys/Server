namespace Raports.Application.Raports.EnqueueDailyRaportGeneration
{
    public record EnqueueDailyRaportCommand(DateTime RaportDate) : IRequest<EnqueueDailyRaportResponse>;
    public record EnqueueDailyRaportResponse(bool Success);

    public class EnqueueDailyRaportCommandValidator : AbstractValidator<EnqueueDailyRaportCommand>
    {
        public EnqueueDailyRaportCommandValidator()
        {

        }
    }
}
