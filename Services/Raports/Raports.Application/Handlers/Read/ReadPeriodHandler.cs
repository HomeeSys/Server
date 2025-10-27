namespace Raports.Application.Handlers.Read;

public class ReadPeriodHandler(RaportsDBContext dBContext) : IRequestHandler<ReadPeriodCommand, ReadPeriodResponse>
{
    public async Task<ReadPeriodResponse> Handle(ReadPeriodCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Periods
            .FirstOrDefaultAsync(x => x.ID == request.ID, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(nameof(Raport), request.ID);
        }

        var dto = entity.Adapt<DefaultPeriodDTO>();

        var response = new ReadPeriodResponse(dto);

        return response;
    }
}

public class ReadAllPeriodHandler(RaportsDBContext dBContext) : IRequestHandler<ReadAllPeriodCommand, ReadAllPeriodResponse>
{
    public async Task<ReadAllPeriodResponse> Handle(ReadAllPeriodCommand request, CancellationToken cancellationToken)
    {
        var entity = await dBContext.Periods
            .ToListAsync(cancellationToken);

        var dto = entity.Adapt<IEnumerable<DefaultPeriodDTO>>();

        var response = new ReadAllPeriodResponse(dto);

        return response;
    }
}