namespace Raports.Application.Handlers.Update;

public class RetryRaportHandler(
    RaportsDBContext dbContext,
    IHubContext<RaportsHub> hub,
    IPublishEndpoint publish,
    ILogger<RetryRaportHandler> logger
    ) : IRequestHandler<RetryRaportCommand, ReadRaportResponse>
{
    public async Task<ReadRaportResponse> Handle(RetryRaportCommand request, CancellationToken cancellationToken)
    {
        var raport = await dbContext.Raports
            .Include(x => x.Status)
            .Include(x => x.Period)
            .Include(x => x.RequestedMeasurements)
            .Include(x => x.RequestedLocations)
            .Include(x => x.MeasurementGroups)
            .FirstOrDefaultAsync(x => x.ID == request.RaportID, cancellationToken);

        if (raport is null)
        {
            logger.LogWarning("Raport {RaportId} not found for retry", request.RaportID);
            throw new EntityNotFoundException(nameof(Raport), request.RaportID);
        }

        if (raport.Status.Name != "Failed")
        {
            logger.LogWarning("Raport {RaportId} cannot be retried. Current status: {Status}", request.RaportID, raport.Status.Name);
            throw new InvalidOperationException($"Only failed reports can be retried. Current status: {raport.Status.Name}");
        }

        var pendingStatus = await dbContext.Statuses.FirstOrDefaultAsync(x => x.Name == "Pending", cancellationToken);
        if (pendingStatus is null)
        {
            logger.LogError("Pending status not found in database");
            throw new InvalidOperationException("Status 'Pending' not found in database");
        }

        if (raport.MeasurementGroups?.Any() == true)
        {
            dbContext.MeasurementGroups.RemoveRange(raport.MeasurementGroups);
            logger.LogInformation("Deleted {Count} measurement groups for Raport {RaportId}", raport.MeasurementGroups.Count, request.RaportID);
        }

        raport.StatusID = pendingStatus.ID;
        raport.Message = string.Empty;
        raport.DocumentHash = Guid.Empty;

        await dbContext.SaveChangesAsync(cancellationToken);

        var dto = raport.Adapt<DefaultRaportDTO>();

        await hub.Clients.All.SendAsync("RaportStatusChanged", dto, cancellationToken);

        var message = new RaportPending()
        {
            Raport = dto
        };

        await publish.Publish(message, cancellationToken);

        logger.LogInformation("Raport {RaportId} retry initiated - status changed to Pending", request.RaportID);

        return new ReadRaportResponse(dto);
    }
}
