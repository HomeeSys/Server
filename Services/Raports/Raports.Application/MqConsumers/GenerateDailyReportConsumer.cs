namespace Raports.Application.MqConsumers;

public class GenerateDailyReportConsumer(ILogger<GenerateDailyReportConsumer> logger, RaportContainer raportContainer, MeasurementPacketGenerator measurementPacketGenerator) : IConsumer<GenerateDailyReportMessage>
{
    public async Task Consume(ConsumeContext<GenerateDailyReportMessage> context)
    {
        logger.LogInformation($"[Log] Consuming 'Generate-Daily-Report' for Date: '{context.Message.RaportDate}' message from RabbitMQ!");

        DateTime raportDate = context.Message.RaportDate;

        var result = await measurementPacketGenerator.ProcessDailyDataForReport(raportDate);
        logger.LogInformation($"[Log] Measurement packets for raport generation are ready!");

        //  Generate daily raport.
        var raport = RaportGenerator.GenerateRaport(result, raportDate);
        logger.LogInformation($"[Log] Raport was generated!");

        bool wasAdded = await raportContainer.UploadDocumentAsync(raport);
        if (wasAdded == true)
        {
            logger.LogInformation($"[Log] Raport uploaded to Azure sucessfully!");
        }
        else
        {
            logger.LogInformation($"[Log] Failed to upload raport to Azure!");
        }

        logger.LogInformation($"[Log] Exiting 'Generate-Daily-Report' consumer!");
    }
}
