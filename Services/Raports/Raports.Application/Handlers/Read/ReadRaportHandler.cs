namespace Raports.Application.Handlers.Read;

public class ReadRaportHandler(RaportsDBContext dBContext) : IRequestHandler<ReadRaportCommand, ReadRaportResponse>
{
    public async Task<ReadRaportResponse> Handle(ReadRaportCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Raports
            .Include(x => x.Period)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(x => x.ID == request.ID, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(nameof(Raport), request.ID);
        }

        var dto = entity.Adapt<DefaultRaportDTO>();

        var response = new ReadRaportResponse(dto);

        return response;
    }
}

public class ReadAllRaportHandler(RaportsDBContext dBContext) : IRequestHandler<ReadAllRaportsCommand, ReadAllRaportsResponse>
{
    public async Task<ReadAllRaportsResponse> Handle(ReadAllRaportsCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Raports
            .Include(x => x.Period)
            .Include(x => x.Status)
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultRaportDTO>>();

        var response = new ReadAllRaportsResponse(dto);

        return response;
    }
}


public class ReadAllRaportsQueryHandler(RaportsDBContext dbcontext) : IRequestHandler<ReadAllRaportsQueryCommand, ReadAllRaportsQueryResponse>
{
    public async Task<ReadAllRaportsQueryResponse> Handle(ReadAllRaportsQueryCommand request, CancellationToken cancellationToken)
    {
        var query = dbcontext.Raports
            .Include(x => x.Status)
            .Include(x => x.Period)
            .AsQueryable();

        if (request.PeriodName is not null)
        {
            query = query.Where(x => x.Period.Name == request.PeriodName);
        }

        if (request.StatusName is not null)
        {
            query = query.Where(x => x.Status.Name == request.StatusName);
        }

        if (request.CreationDateFrom is not null)
        {
            query = query.Where(x => x.RaportCreationDate >= request.CreationDateFrom);
        }

        if (request.CreationDateTo is not null)
        {
            query = query.Where(x => x.RaportCreationDate <= request.CreationDateTo);
        }

        string sortOrder = "asc";
        if (!string.IsNullOrEmpty(request.SortOrder))
        {
            sortOrder = request.SortOrder;
        }

        if (sortOrder == "asc")
        {
            query = query.OrderBy(x => x.RaportCreationDate);
        }
        else
        {
            query = query.OrderByDescending(x => x.RaportCreationDate);
        }

        var paginetedOutput = await PaginatedList<Raport>.Create(query, request.Page, request.PageSize, await dbcontext.Raports.CountAsync());

        var requestsDto = paginetedOutput.Items.Adapt<List<DefaultRaportDTO>>();

        var paginatedDto = new PaginatedList<DefaultRaportDTO>(requestsDto, paginetedOutput.Page, paginetedOutput.PageSize, paginetedOutput.TotalCount, paginetedOutput.AbsoluteCount);

        var response = new ReadAllRaportsQueryResponse(paginatedDto);

        return response;
    }
}