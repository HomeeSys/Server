namespace Raports.Application.Handlers.Read;

public class ReadRaportHandler(RaportsDBContext dBContext) : IRequestHandler<ReadRaportCommand, ReadRaportResponse>
{
    public async Task<ReadRaportResponse> Handle(ReadRaportCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Raports
            .Include(x => x.Request)
            .Include(x => x.Period)
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
            .Include(x => x.Request)
            .Include(x => x.Period)
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultRaportDTO>>();

        var response = new ReadAllRaportsResponse(dto);

        return response;
    }
}