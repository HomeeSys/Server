namespace Raports.Application.Handlers.Read;

public class ReadRequestHandler(RaportsDBContext dBContext) : IRequestHandler<ReadRequestCommand, ReadRequestResponse>
{
    public async Task<ReadRequestResponse> Handle(ReadRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Requests
            .Include(x => x.Period)
            .Include(x => x.Raport)
            .Include(x => x.Status)
            .FirstOrDefaultAsync(x => x.ID == request.ID, cancellationToken: cancellationToken);
        //if (entity == null || entity.Status.Name == "Deleted")
        if (entity == null)
        {
            throw new EntityNotFoundException(nameof(Raport), request.ID);
        }

        var dto = entity.Adapt<DefaultRequestDTO>();

        var response = new ReadRequestResponse(dto);

        return response;
    }
}

public class ReadAllRequestHandler(RaportsDBContext dBContext) : IRequestHandler<ReadAllRequestCommand, ReadAllRequestResponse>
{
    public async Task<ReadAllRequestResponse> Handle(ReadAllRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Requests
            .Include(x => x.Period)
            .Include(x => x.Raport)
            .Include(x => x.Status)
            //.Where(x => x.Status.Name != "Deleted")
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultRequestDTO>>();

        var response = new ReadAllRequestResponse(dto);

        return response;
    }
}

public class ReadAllRequstsQueryHandler(RaportsDBContext dbcontext) : IRequestHandler<ReadAllRequestsQueryCommand, PaginatedList<DefaultRequestDTO>>
{
    public async Task<PaginatedList<DefaultRequestDTO>> Handle(ReadAllRequestsQueryCommand request, CancellationToken cancellationToken)
    {
        var query = dbcontext.Requests
            .Include(x => x.Status)
            .Include(x => x.Period)
            .Include(x => x.Raport)
            //.Where(x => x.Status.Name != "Deleted")
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
            query = query.Where(x => x.RequestCreationDate >= request.CreationDateFrom);
        }

        if (request.CreationDateTo is not null)
        {
            query = query.Where(x => x.RequestCreationDate <= request.CreationDateTo);
        }

        string sortOrder = "asc";
        if (!string.IsNullOrEmpty(request.SortOrder))
        {
            sortOrder = request.SortOrder;
        }

        if (sortOrder == "asc")
        {
            query = query.OrderBy(x => x.RequestCreationDate);
        }
        else
        {
            query = query.OrderByDescending(x => x.RequestCreationDate);
        }

        var paginetedOutput = await PaginatedList<Request>.Create(query, request.Page, request.PageSize, await dbcontext.Requests.CountAsync());

        var requestsDto = paginetedOutput.Items.Adapt<List<DefaultRequestDTO>>();

        var paginatedDto = new PaginatedList<DefaultRequestDTO>(requestsDto, paginetedOutput.Page, paginetedOutput.PageSize, paginetedOutput.TotalCount, paginetedOutput.AbsoluteCount);

        return paginatedDto;
    }
}