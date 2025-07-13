namespace Measurements.Infrastructure.Exceptions;

internal class FailedToCreateMeasurementException : Exception
{
    public FailedToCreateMeasurementException() : base("Failed to create Measurement")
    {
        
    }
}
