namespace Raports.Application.Handlers.Update;

public class UpdateRequestStatusHandler(RaportsDBContext dbcontext, IHubContext<RaportsHub> hub) : IRequestHandler<UpdateRequestStatusCommand, ReadRequestResponse>
{
    public async Task<ReadRequestResponse> Handle(UpdateRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var statusEntity = await dbcontext.RequestStatuses.FirstOrDefaultAsync(x => x.Name == request.UpdateDTO.UpdatedStatusName);
        if (statusEntity is null)
        {
            throw new EntityNotFoundException(nameof(RequestStatus), request.UpdateDTO.UpdatedStatusName);
        }

        var requestEntity = await dbcontext.Requests
            .Include(x => x.Status)
            .Include(x => x.Raport)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == request.RequestID);
        if (requestEntity is null)
        {
            throw new EntityNotFoundException(nameof(Request), request.RequestID);
        }

        requestEntity.StatusID = statusEntity.ID;

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

public class UpdateRequestHandler(RaportsDBContext dbcontext, IHubContext<RaportsHub> hub) : IRequestHandler<UpdateRequestCommand, ReadRequestResponse>
{
    public async Task<ReadRequestResponse> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
    {
        Period? periodEntity = null;

        var requestEntity = await dbcontext.Requests
            .Include(x => x.Status)
            .Include(x => x.Raport)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == request.RequestID);
        if (requestEntity is null)
        {
            throw new EntityNotFoundException(nameof(Request), request.RequestID);
        }

        if (requestEntity.RaportID == request.UpdateDTO.UpdatedRaportID)
        {
            throw new Exception();
        }

        //  Update raport
        if (request.UpdateDTO.UpdatedRaportID is not null)
        {
            var raportEntity = await dbcontext.Raports.FirstOrDefaultAsync(x => x.ID == (int)request.UpdateDTO.UpdatedRaportID);
            if (raportEntity is null)
            {
                throw new EntityNotFoundException(nameof(Raport), request.UpdateDTO.UpdatedRaportID);
            }

            requestEntity.RaportID = raportEntity.ID;
        }

        //  Update period
        if (request.UpdateDTO.UpdatedPeriodID is not null)
        {
            periodEntity = await dbcontext.Periods.FirstOrDefaultAsync(x => x.ID == (int)request.UpdateDTO.UpdatedPeriodID);
            if (periodEntity is null)
            {
                throw new EntityNotFoundException(nameof(Period), request.UpdateDTO.UpdatedPeriodID);
            }

            requestEntity.PeriodID = periodEntity.ID;
        }

        if (request.UpdateDTO.UpdatedStartDate is not null && request.UpdateDTO.UpdatedEndDate is not null)
        {
            DateTime start = (DateTime)request.UpdateDTO.UpdatedStartDate;
            DateTime end = (DateTime)request.UpdateDTO.UpdatedEndDate;

            //  Remove everyting after hours.
            DateTime endDate = new DateTime(
                end.Year,
                end.Month,
                end.Day,
                end.Hour,
                0,
                0);

            DateTime startDate = new DateTime(
                start.Year,
                start.Month,
                start.Day,
                start.Hour,
                0,
                0);

            requestEntity.StartDate = startDate;
            requestEntity.EndDate = endDate;
        }

        var entry = dbcontext.Entry(requestEntity);
        dbcontext.ChangeTracker.DetectChanges();

        var wasChanged = entry.Properties.Any(p => p.IsModified) || entry.ComplexProperties.Any(c => c.IsModified);

        var requestDTO = requestEntity.Adapt<DefaultRequestDTO>();
        var response = new ReadRequestResponse(requestDTO);

        if (wasChanged)
        {
            await dbcontext.SaveChangesAsync(cancellationToken);
            await hub.Clients.All.SendAsync("RequestUpdated", requestDTO, cancellationToken);
        }

        return response;
    }
}

public class UpdateRequestRaportHandler(RaportsDBContext dbcontext, IHubContext<RaportsHub> hub) : IRequestHandler<UpdateRequestRaportCommand, ReadRequestResponse>
{
    public async Task<ReadRequestResponse> Handle(UpdateRequestRaportCommand request, CancellationToken cancellationToken)
    {
        var raportEntity = await dbcontext.Raports.FirstOrDefaultAsync(x => x.ID == request.UpdateDTO.UpdatedRaportID);
        if (raportEntity is null)
        {
            throw new EntityNotFoundException(nameof(Raport), request.UpdateDTO.UpdatedRaportID);
        }

        var requestEntity = await dbcontext.Requests
            .Include(x => x.Status)
            .Include(x => x.Raport)
            .Include(x => x.Period)
            .FirstOrDefaultAsync(x => x.ID == request.RequestID);
        if (requestEntity is null)
        {
            throw new EntityNotFoundException(nameof(Request), request.RequestID);
        }

        requestEntity.RaportID = request.UpdateDTO.UpdatedRaportID;

        var entry = dbcontext.Entry(requestEntity);
        dbcontext.ChangeTracker.DetectChanges();

        var wasChanged = entry.Properties.Any(p => p.IsModified) || entry.ComplexProperties.Any(c => c.IsModified);

        var requestDTO = requestEntity.Adapt<DefaultRequestDTO>();
        var response = new ReadRequestResponse(requestDTO);

        if (wasChanged)
        {
            await dbcontext.SaveChangesAsync(cancellationToken);
            await hub.Clients.All.SendAsync("RequestRaportChanged", requestDTO, cancellationToken);
        }

        return response;
    }
}