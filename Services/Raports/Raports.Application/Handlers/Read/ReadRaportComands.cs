namespace Raports.Application.Handlers.Read;

public record ReadRaportCommand(int ID) : IRequest<ReadRaportResponse>;
public record ReadRaportResponse(DefaultRaportDTO RaportDTO);
public class ReadRaportCommandValidator : AbstractValidator<ReadRaportCommand>
{
    public ReadRaportCommandValidator() { }
}

public record ReadAllRaportsCommand() : IRequest<ReadAllRaportsResponse>;
public record ReadAllRaportsResponse(IEnumerable<DefaultRaportDTO> RaportDTOs);
public class ReadAllRaportsCommandValidator : AbstractValidator<ReadAllRaportsCommand>
{
    public ReadAllRaportsCommandValidator() { }
}
