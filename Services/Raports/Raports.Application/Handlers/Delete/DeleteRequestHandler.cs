namespace Raports.Application.Handlers.Delete;

public class DeleteRequestHandler(RaportsDBContext dbcontext, IHubContext<RaportsHub> hub) : IRequestHandler<SoftDeleteRequestCommand, ReadRequestResponse>
{
    public async Task<ReadRequestResponse> Handle(SoftDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var requestEntity = await dbcontext.Requests
            .Include(x => x.Status)
            .Include(x => x.Period)
            .Include(x => x.Raport)
            .FirstOrDefaultAsync(x => x.ID == request.RequestID);
        if (requestEntity is null)
        {
            throw new EntityNotFoundException(nameof(Request), request.RequestID);
        }

        var deleteStatusEntity = await dbcontext.RequestStatuses.FirstOrDefaultAsync(x => x.Name == "Deleted");
        if (deleteStatusEntity is null)
        {
            throw new EntityNotFoundException(nameof(RequestStatus), "Deleted");
        }

        requestEntity.StatusID = deleteStatusEntity.ID;

        var entry = dbcontext.Entry(requestEntity);
        dbcontext.ChangeTracker.DetectChanges();

        var wasChanged = entry.Properties.Any(p => p.IsModified) || entry.ComplexProperties.Any(c => c.IsModified);

        var requestDTO = requestEntity.Adapt<DefaultRequestDTO>();
        var response = new ReadRequestResponse(requestDTO);

        if (wasChanged)
        {
            await dbcontext.SaveChangesAsync(cancellationToken);
            await hub.Clients.All.SendAsync("RequestStatusChanged", requestDTO, cancellationToken);
        }

        return response;
    }
}
