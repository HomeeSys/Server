namespace Devices.Application.Devices.GetDevice;

public class GetMonitoredLocationsHandler(DevicesDBContext context) : IRequestHandler<GetMonitoredLocationsCommand, GetMonitoredLocationsResponse>
{
    public async Task<GetMonitoredLocationsResponse> Handle(GetMonitoredLocationsCommand request, CancellationToken cancellationToken)
    {
        var monitoredLocations = await context.Devices
            .Include(d => d.Location)
            .Include(d => d.Status)
            .Where(d => d.Status.Type == "Online")
            .Select(d => d.Location)
            .Distinct()
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        var dtos = monitoredLocations.Adapt<List<LocationDTO>>();

        var response = new GetMonitoredLocationsResponse(dtos);

        return response;
    }
}
