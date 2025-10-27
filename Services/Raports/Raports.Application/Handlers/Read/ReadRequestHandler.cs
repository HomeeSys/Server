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
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultRequestDTO>>();

        var response = new ReadAllRequestResponse(dto);

        return response;
    }
}