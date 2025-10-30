namespace Raports.Application.Handlers.Read;

public class ReadRaportStatusHandler(RaportsDBContext dBContext) : IRequestHandler<ReadRaportStatusesCommand, ReadRaportStatusesResponse>
{
    public async Task<ReadRaportStatusesResponse> Handle(ReadRaportStatusesCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.RequestStatuses
            .FirstOrDefaultAsync(x => x.ID == request.ID, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(nameof(Raport), request.ID);
        }

        var dto = entity.Adapt<DefaultRequestStatusDTO>();

        var response = new ReadRaportStatusesResponse(dto);

        return response;
    }
}

public class ReadAllRaportsStatusesHandler(RaportsDBContext dBContext) : IRequestHandler<ReadAllRaportStatusesCommand, ReadAllRaportStatusesResponse>
{
    public async Task<ReadAllRaportStatusesResponse> Handle(ReadAllRaportStatusesCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.RequestStatuses
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultRequestStatusDTO>>();

        var response = new ReadAllRaportStatusesResponse(dto);

        return response;
    }
}
