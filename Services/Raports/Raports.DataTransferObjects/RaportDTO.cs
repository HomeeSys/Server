namespace Raports.DataTransferObjects;

public record DefaultRaportDTO(
    int ID,
    DateTime CreationDate,
    DateTime StartDate,
    DateTime EndDate,
    DefaultPeriodDTO Period,
    RequestDTONoRaport Request
);

public record RaportDTONoRequest(
    int ID,
    DateTime CreationDate,
    DateTime StartDate,
    DateTime EndDate,
    DefaultPeriodDTO Period
);