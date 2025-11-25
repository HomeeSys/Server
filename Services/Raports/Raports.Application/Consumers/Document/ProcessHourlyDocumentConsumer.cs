namespace Raports.Application.Consumers.Document;

internal class ProcessHourlyDocumentConsumer(ILogger<ProcessHourlyDocumentConsumer> logger, IPublishEndpoint publish) : IConsumer<RaportProduceDocument>
{
    public async Task Consume(ConsumeContext<RaportProduceDocument> context)
    {
        logger.LogInformation($"Creating document for Hourly raport");

        var message = new RaportReady()
        {
            Raport = context.Message.Raport
        };

        await publish.Publish(message, context =>
        {
            context.Headers.Set("PeriodName", message.Raport.Period.Name);
        });
    }
}
