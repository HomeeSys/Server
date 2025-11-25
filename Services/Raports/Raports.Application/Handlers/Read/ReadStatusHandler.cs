namespace Raports.Application.Handlers.Read;

public class ReadAllRaportsStatusesHandler(RaportsDBContext dBContext) : IRequestHandler<ReadAllStatusesCommand, ReadAllStatusesResponse>
{
    public async Task<ReadAllStatusesResponse> Handle(ReadAllStatusesCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Statuses
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultStatusDTO>>();

        var response = new ReadAllStatusesResponse(dto);

        return response;
    }
}
