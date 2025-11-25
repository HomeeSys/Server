namespace Raports.Application.Handlers.Read;

public class ReadAllPeriodHandler(RaportsDBContext database) : IRequestHandler<ReadAllPeriodCommand, ReadAllPeriodResponse>
{
    public async Task<ReadAllPeriodResponse> Handle(ReadAllPeriodCommand request, CancellationToken cancellationToken)
    {
        var periods = await database.Periods.ToListAsync(cancellationToken);

        var dtos = periods.Adapt<IEnumerable<DefaultPeriodDTO>>();

        var response = new ReadAllPeriodResponse(dtos);

        return response;
    }
}