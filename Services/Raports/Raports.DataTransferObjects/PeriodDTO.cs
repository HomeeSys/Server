namespace Raports.DataTransferObjects;

public record DefaultPeriodDTO(
    int ID,
    string Name,
    TimeSpan TimeFrame,
    int MaxAcceptableMissingTimeFrame
);