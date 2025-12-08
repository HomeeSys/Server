namespace Raports.Application.Handlers.Read;

public class DownloadRaportHandler(
    RaportsDBContext dbContext,
    BlobContainerClient blobContainerClient,
    ILogger<DownloadRaportHandler> logger) : IRequestHandler<DownloadRaportCommand, DownloadRaportResponse>
{
    public async Task<DownloadRaportResponse> Handle(DownloadRaportCommand request, CancellationToken cancellationToken)
    {
        var raport = await dbContext.Raports
            .Include(x => x.Status)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == request.RaportID, cancellationToken);

        if (raport is null)
        {
            logger.LogWarning("Raport {RaportId} not found", request.RaportID);
            throw new EntityNotFoundException(nameof(Raport), request.RaportID);
        }

        if (raport.Status.Name != "Completed")
        {
            logger.LogWarning("Raport {RaportId} is not completed. Current status: {Status}", request.RaportID, raport.Status.Name);
            throw new InvalidOperationException($"Raport {request.RaportID} is not completed. Current status: {raport.Status.Name}");
        }

        if (raport.DocumentHash == Guid.Empty)
        {
            logger.LogWarning("Raport {RaportId} does not have a document hash", request.RaportID);
            throw new InvalidOperationException($"Raport {request.RaportID} does not have an associated document");
        }

        var fileName = $"{raport.DocumentHash}.pdf";
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            logger.LogError("Blob {BlobName} not found in Azure Storage for Raport {RaportId}", fileName, request.RaportID);
            throw new InvalidOperationException($"Report file not found in storage for Raport {request.RaportID}");
        }

        var stream = new MemoryStream();
        await blobClient.DownloadToAsync(stream, cancellationToken);
        stream.Position = 0;

        logger.LogInformation("Successfully downloaded raport {RaportId} from Azure Blob Storage", request.RaportID);

        return new DownloadRaportResponse(stream, fileName, "application/pdf");
    }
}
