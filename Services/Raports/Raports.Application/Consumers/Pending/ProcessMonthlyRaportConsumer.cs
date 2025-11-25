namespace Raports.Application.Consumers.Pending;

internal class ProcessMonthlyRaportConsumer(ILogger<ProcessMonthlyRaportConsumer> logger, IPublishEndpoint publish) : IConsumer<RaportPending>
{
    public async Task Consume(ConsumeContext<RaportPending> context)
    {
        logger.LogInformation($"Processing Monthly raport");

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
