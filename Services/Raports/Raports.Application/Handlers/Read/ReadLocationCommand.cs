namespace Raports.Application.Handlers.Read;

public record ReadAllLocationsResponse(IEnumerable<DefaultLocationDTO> LocationsDTOs);
public record ReadAllLocationsCommand() : IRequest<ReadAllLocationsResponse>;
public class ReadAllLocationsCommandValidator : AbstractValidator<ReadAllLocationsCommand>
{
    public ReadAllLocationsCommandValidator() { }
}
