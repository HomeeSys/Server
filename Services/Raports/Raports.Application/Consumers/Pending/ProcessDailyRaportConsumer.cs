namespace Raports.Application.Consumers.Pending;

internal class ProcessDailyRaportConsumer(ILogger<ProcessDailyRaportConsumer> logger, IPublishEndpoint publish) : IConsumer<RaportPending>
{
    public async Task Consume(ConsumeContext<RaportPending> context)
    {
        logger.LogInformation($"Processing Daily raport");

        var message = new RaportToSummary()
        {
            Raport = context.Message.Raport
        };

        await publish.Publish(message, context =>
        {
            context.Headers.Set("PeriodName", message.Raport.Period.Name);
        });
    }
}
