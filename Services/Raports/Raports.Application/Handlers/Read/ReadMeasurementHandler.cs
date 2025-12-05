namespace Raports.Application.Handlers.Read;

public class ReadAllMeasurementsHandler(RaportsDBContext database) : IRequestHandler<ReadAllMeasurementsCommand, ReadAllMeasurementsResponse>
{
    public async Task<ReadAllMeasurementsResponse> Handle(ReadAllMeasurementsCommand request, CancellationToken cancellationToken)
    {
        var measurements = await database.Measurements.ToListAsync(cancellationToken);

        var dtos = measurements.Adapt<IEnumerable<DefaultMeasurementDTO>>();

        var response = new ReadAllMeasurementsResponse(dtos);

        return response;
    }
}