namespace Measurements.Infrastructure.Exceptions;

internal class MeasurementDoesntExistException : Exception
{
    public MeasurementDoesntExistException(object key) : base($"Measurement with '{key}' doesn't exist")
    {
        
    }
}
