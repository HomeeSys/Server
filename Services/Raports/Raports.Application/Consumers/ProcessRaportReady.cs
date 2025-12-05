namespace Raports.Application.Consumers;

internal class ProcessRaportReady(ILogger<ProcessRaportReady> logger) : IConsumer<RaportReady>
{
    public async Task Consume(ConsumeContext<RaportReady> context)
    {
        logger.LogInformation($"Raport ready");
    }
}
