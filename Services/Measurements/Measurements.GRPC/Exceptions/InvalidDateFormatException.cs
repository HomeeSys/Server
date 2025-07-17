namespace Measurements.GRPC.Exceptions;

public class InvalidDateFormatException : Exception
{
    public InvalidDateFormatException(string invalidInput) : base($"Failed to convert '{invalidInput}' value to proper DateTime type!") { }
}
