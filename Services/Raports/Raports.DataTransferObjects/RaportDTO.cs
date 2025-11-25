namespace Raports.DataTransferObjects;

public record DefaultRaportDTO(
    int ID,
    DateTime RaportCreationDate,
    DateTime RaportCompletedDate,
    DateTime StartDate,
    DateTime EndDate,
    string Message,
    DefaultPeriodDTO Period,
    DefaultStatusDTO Status,
    ICollection<DefaultMeasurementDTO> RequestedMeasurements,
    ICollection<DefaultLocationDTO> RequestedLocations
);
