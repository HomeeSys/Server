namespace Raports.Application.Consumers;

internal class RaportFailedConsumer(ILogger<RaportFailedConsumer> logger,
                                       IPublishEndpoint publish,
                                       RaportsDBContext database,
                                       IMemoryCache cashe,
                                       MeasurementService.MeasurementServiceClient measurementsGRPC,
                                       DevicesService.DevicesServiceClient deviceGRPC) : IConsumer<RaportFailed>
{
    public async Task Consume(ConsumeContext<RaportFailed> context)
    {
        logger.LogInformation("RaportFailedConsumer: handling failure for RaportID={RaportId}", context.Message.Raport?.ID);

        var ct = context.CancellationToken;

        if (context.Message is null)
        {
            logger.LogWarning("Received empty RaportFailed message");
            return;
        }

        var raportDto = context.Message.Raport;
        if (raportDto is null)
        {
            logger.LogWarning("RaportFailed message does not contain Raport DTO");
            return;
        }

        try
        {
            var raport = await database.Raports.FirstOrDefaultAsync(r => r.ID == raportDto.ID, ct);
            if (raport is null)
            {
                logger.LogWarning("Raport with ID {RaportId} not found in DB", raportDto.ID);
                return;
            }

            var failedStatus = await database.Statuses.FirstOrDefaultAsync(s => s.Name == "Failed", ct) ?? await database.Statuses.FirstOrDefaultAsync(ct);
            if (failedStatus is not null)
            {
                raport.StatusID = failedStatus.ID;
            }

            // set message and completion date from incoming message
            raport.Message = context.Message.Description ?? raport.Message;
            raport.RaportCompletedDate = context.Message.FailedDate == default ? DateTime.UtcNow : context.Message.FailedDate;

            await database.SaveChangesAsync(ct);

            logger.LogInformation("Raport ID {RaportId} updated to Failed in DB", raport.ID);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling RaportFailed for RaportID={RaportId}", context.Message.Raport?.ID);
            throw;
        }
    }
}
