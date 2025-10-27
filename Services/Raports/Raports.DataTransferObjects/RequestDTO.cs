namespace Raports.DataTransferObjects;

public record CreateRequestDTO(
    DateTime StartDate,
    DateTime EndDate,
    string PeriodName
);

public record DefaultRequestDTO(
    int ID,
    DateTime RequestCreationDate,
    DateTime StartDate,
    DateTime EndDate,
    DefaultRequestStatusDTO Status,
    RaportDTONoRequest? Raport,
    DefaultPeriodDTO Period
);

public record RequestDTONoRaport(
    int ID,
    DateTime RequestCreationDate,
    DateTime StartDate,
    DateTime EndDate,
    DefaultRequestStatusDTO Status,
    DefaultPeriodDTO Period
);
