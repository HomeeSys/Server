namespace Raports.Application.Consumers;

internal class RaportFailedConsumer(ILogger<RaportFailedConsumer> logger,
                                    RaportsDBContext database,
                                    IHubContext<RaportsHub> hub) : IConsumer<RaportFailed>
{
    public async Task Consume(ConsumeContext<RaportFailed> context)
    {
        var raportDto = context.Message.Raport;
        var description = context.Message.Description;
        var raportId = raportDto?.ID;

        logger.LogInformation("RaportFailedConsumer: handling failure for RaportID={RaportId}", raportId);

        try
        {
            var ct = context.CancellationToken;

            if (raportDto is null)
            {
                logger.LogWarning("RaportFailed message does not contain Raport DTO");
                throw new InvalidOperationException("RaportFailed message must contain a Raport DTO");
            }

            logger.LogWarning("Raport {RaportId} failed: {Description}", raportId, context.Message.Description);

            var failedStatus = await database.Statuses.FirstOrDefaultAsync(x => x.Name == "Failed", ct);
            if (failedStatus is null)
            {
                logger.LogError("Failed status not found in database");
                throw new InvalidOperationException("Status 'Failed' not found in database");
            }

            var raport = await database.Raports
                .Include(x => x.RequestedLocations)
                .Include(x => x.RequestedMeasurements)
                .Include(x => x.Period)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.ID == raportDto.ID, ct);

            if (raport is null)
            {
                logger.LogWarning("Raport {RaportId} not found in database", raportId);
                return;
            }

            if (raport.Status.Name == "Failed")
            {
                logger.LogInformation("Raport {RaportId} is already in Failed status, skipping update", raportId);
                return;
            }

            raport.StatusID = failedStatus.ID;
            raport.Message = description;

            await database.SaveChangesAsync(ct);

            var dto = raport.Adapt<DefaultRaportDTO>();
            await hub.Clients.All.SendAsync("RaportStatusChanged", dto, ct);

            logger.LogInformation("Raport {RaportId} status updated to Failed", raportId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling RaportFailed for RaportID={RaportId}", raportId);
            throw;
        }
    }
}
