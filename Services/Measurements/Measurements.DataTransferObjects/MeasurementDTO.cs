namespace Measurements.DataTransferObjects;

public record CreateMeasurementDTO(double Value, string Unit);
public record MeasurementDTO(double Value, string Unit);