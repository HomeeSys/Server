namespace Measurements.Infrastructure.Exceptions;

internal class FailedToDeleteMeasurement : Exception
{
    public FailedToDeleteMeasurement() : base("Failed to delete MeasurementSet")
    {
        
    }
}
