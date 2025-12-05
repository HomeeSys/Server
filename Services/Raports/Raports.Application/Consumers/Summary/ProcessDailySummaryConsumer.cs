namespace Raports.Application.Consumers.Summary;

internal class ProcessDailySummaryConsumer(ILogger<ProcessDailySummaryConsumer> logger, IPublishEndpoint publish) : IConsumer<RaportToSummary>
{
    public async Task Consume(ConsumeContext<RaportToSummary> context)
    {
        logger.LogInformation($"Generating Daily summary");

        var message = new RaportProduceDocument()
        {
            Raport = context.Message.Raport
        };

        await publish.Publish(message, context =>
        {
            context.Headers.Set("PeriodName", message.Raport.Period.Name);
        });
    }
}
