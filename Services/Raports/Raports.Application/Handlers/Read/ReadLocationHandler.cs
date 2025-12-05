namespace Raports.Application.Handlers.Read;

public class ReadAllLocationsHandler(RaportsDBContext database) : IRequestHandler<ReadAllLocationsCommand, ReadAllLocationsResponse>
{
    public async Task<ReadAllLocationsResponse> Handle(ReadAllLocationsCommand request, CancellationToken cancellationToken)
    {
        var locations = await database.Locations.ToListAsync(cancellationToken);

        var dtos = locations.Adapt<IEnumerable<DefaultLocationDTO>>();

        var response = new ReadAllLocationsResponse(dtos);

        return response;
    }
}