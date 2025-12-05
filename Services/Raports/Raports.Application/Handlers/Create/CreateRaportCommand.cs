namespace Raports.Application.Handlers.Create;

public record CreateRaportResponse(DefaultRaportDTO RaportDTO);
public record CreateRaportCommand(DateTime StartDate, DateTime EndDate, int PeriodID, int[] RequestedLocationsIDs, int[] RequestedMeasurementsIDs) : IRequest<CreateRaportResponse>;
public class CreateRaportCommandValidator : AbstractValidator<CreateRaportCommand>
{
    public CreateRaportCommandValidator()
    {

    }
}