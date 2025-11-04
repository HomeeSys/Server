namespace Raports.Application.Raports.EnqueueDailyRaportGeneration
{
    public class EnqueueDailyRaportGenerationHandler(IPublishEndpoint publisher) : IRequestHandler<EnqueueDailyRaportCommand, EnqueueDailyRaportResponse>
    {
        public async Task<EnqueueDailyRaportResponse> Handle(EnqueueDailyRaportCommand request, CancellationToken cancellationToken)
        {
            EnqueueDailyRaportGenerationMessage newEvent = new EnqueueDailyRaportGenerationMessage()
            {
                RaportDate = request.RaportDate,
            };

            try
            {
                await publisher.Publish(newEvent, cancellationToken);
            }
            catch (Exception)
            {
                return new EnqueueDailyRaportResponse(false);
            }

            return new EnqueueDailyRaportResponse(true);
        }
    }
}
