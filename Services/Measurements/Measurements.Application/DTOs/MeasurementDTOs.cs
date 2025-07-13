namespace Measurements.Application.DTOs;

public record CreateMeasurementDTO(double Value, string Unit);
public record MeasurementDTO(double Value, string Unit);