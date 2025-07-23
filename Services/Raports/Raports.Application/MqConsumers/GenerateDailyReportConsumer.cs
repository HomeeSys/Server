using Microsoft.Extensions.Logging;
using Raports.Infrastructure.Generators;

namespace Raports.Application.MqConsumers;

public class GenerateDailyReportConsumer(ILogger<GenerateDailyReportConsumer> logger, RaportContainer raportContainer, MeasurementPacketGenerator measurementPacketGenerator) : IConsumer<GenerateDailyReportMessage>
{
    public async Task Consume(ConsumeContext<GenerateDailyReportMessage> context)
    {
        DateTime raportDate = context.Message.RaportDate;

        var result = await measurementPacketGenerator.ProcessDailyDataForReport(raportDate);

        //  Generate daily raport.
        var raport = RaportGenerator.GenerateRaport(result, raportDate);

        bool wasAdded = await raportContainer.UploadDocumentAsync(raport);
        if (wasAdded == true)
        {
            logger.LogInformation($"[Raports] Raport for date '{raportDate}' generated sucessfully!");
        }
        else
        {
            logger.LogInformation($"[Raports] Failed to generate raport for date '{raportDate}'");
        }
    }
}
