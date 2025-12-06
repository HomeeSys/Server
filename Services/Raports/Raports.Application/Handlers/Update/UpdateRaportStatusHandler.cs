namespace Raports.Application.Handlers.Update;

public class UpdateRaportStatusHandler(
    RaportsDBContext dbcontext,
    IHubContext<RaportsHub> hub,
    IPublishEndpoint publish
    ) : IRequestHandler<UpdateRaportStatusCommand, ReadRaportResponse>
{
    public async Task<ReadRaportResponse> Handle(UpdateRaportStatusCommand request, CancellationToken cancellationToken)
    {
        var statusEntity = await dbcontext.Statuses.FirstOrDefaultAsync(x => x.ID == request.StatusID);
        if (statusEntity is null)
        {
            throw new EntityNotFoundException(nameof(Status), request.RaportID);
        }

        var requestEntity = await dbcontext.Raports
            .Include(x => x.Status)
            .Include(x => x.Period)
            .Include(x => x.RequestedMeasurements)
            .Include(x => x.RequestedLocations)
            .FirstOrDefaultAsync(x => x.ID == request.RaportID);
        if (requestEntity is null)
        {
            throw new EntityNotFoundException(nameof(Raport), request.RaportID);
        }

        requestEntity.StatusID = statusEntity.ID;

        var entry = dbcontext.Entry(requestEntity);
        dbcontext.ChangeTracker.DetectChanges();

        var wasChanged = entry.Properties.Any(p => p.IsModified) || entry.ComplexProperties.Any(c => c.IsModified);

        var dto = requestEntity.Adapt<DefaultRaportDTO>();
        var response = new ReadRaportResponse(dto);

        if (wasChanged)
        {
            await dbcontext.SaveChangesAsync(cancellationToken);
            await hub.Clients.All.SendAsync("RaportStatusChanged", dto, cancellationToken);

            if (requestEntity.Status.Name == "Pending")
            {
                var message = new RaportPending()
                {
                    Raport = dto
                };

                await publish.Publish(message, cancellationToken);
            }
        }

        return response;
    }
}

public class UpdateRaportHandler(RaportsDBContext dbcontext, IHubContext<RaportsHub> hub) : IRequestHandler<UpdateRaportCommand, ReadRaportResponse>
{
    public Task<ReadRaportResponse> Handle(UpdateRaportCommand request, CancellationToken cancellationToken)
    {
        return null;
        //Period? periodEntity = null;

        //var requestEntity = await dbcontext.Requests
        //    .Include(x => x.Status)
        //    .Include(x => x.Raport)
        //    .Include(x => x.Period)
        //    .FirstOrDefaultAsync(x => x.ID == request.RequestID);
        //if (requestEntity is null)
        //{
        //    throw new EntityNotFoundException(nameof(Request), request.RequestID);
        //}

        //if (requestEntity.RaportID == request.UpdateDTO.UpdatedRaportID)
        //{
        //    throw new Exception();
        //}

        ////  Update raport
        //if (request.UpdateDTO.UpdatedRaportID is not null)
        //{
        //    var raportEntity = await dbcontext.Raports.FirstOrDefaultAsync(x => x.ID == (int)request.UpdateDTO.UpdatedRaportID);
        //    if (raportEntity is null)
        //    {
        //        throw new EntityNotFoundException(nameof(Raport), request.UpdateDTO.UpdatedRaportID);
        //    }

        //    requestEntity.RaportID = raportEntity.ID;
        //}

        ////  Update period
        //if (request.UpdateDTO.UpdatedPeriodID is not null)
        //{
        //    periodEntity = await dbcontext.Periods.FirstOrDefaultAsync(x => x.ID == (int)request.UpdateDTO.UpdatedPeriodID);
        //    if (periodEntity is null)
        //    {
        //        throw new EntityNotFoundException(nameof(Period), request.UpdateDTO.UpdatedPeriodID);
        //    }

        //    requestEntity.PeriodID = periodEntity.ID;
        //}

        //if (request.UpdateDTO.UpdatedStartDate is not null && request.UpdateDTO.UpdatedEndDate is not null)
        //{
        //    DateTime start = (DateTime)request.UpdateDTO.UpdatedStartDate;
        //    DateTime end = (DateTime)request.UpdateDTO.UpdatedEndDate;

        //    //  Remove everyting after hours.
        //    DateTime endDate = new DateTime(
        //        end.Year,
        //        end.Month,
        //        end.Day,
        //        end.Hour,
        //        0,
        //        0);

        //    DateTime startDate = new DateTime(
        //        start.Year,
        //        start.Month,
        //        start.Day,
        //        start.Hour,
        //        0,
        //        0);

        //    requestEntity.StartDate = startDate;
        //    requestEntity.EndDate = endDate;
        //}

        //var entry = dbcontext.Entry(requestEntity);
        //dbcontext.ChangeTracker.DetectChanges();

        //var wasChanged = entry.Properties.Any(p => p.IsModified) || entry.ComplexProperties.Any(c => c.IsModified);

        //var requestDTO = requestEntity.Adapt<DefaultRequestDTO>();
        //var response = new ReadRequestResponse(requestDTO);

        //if (wasChanged)
        //{
        //    await dbcontext.SaveChangesAsync(cancellationToken);
        //    await hub.Clients.All.SendAsync("RequestUpdated", requestDTO, cancellationToken);
        //}

        //return response;
    }
}