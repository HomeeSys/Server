namespace Devices.Application.Devices.GetDevice;

public record GetMonitoredLocationsCommand() : IRequest<GetMonitoredLocationsResponse>;
public record GetMonitoredLocationsResponse(IEnumerable<LocationDTO> Locations);

public class GetMonitoredLocationsCommandValidator : AbstractValidator<GetMonitoredLocationsCommand>
{
    public GetMonitoredLocationsCommandValidator()
    {
    }
}
