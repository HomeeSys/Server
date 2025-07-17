namespace Measurements.GRPC.Exceptions
{
    public class InvalidIdentifierFormatException : Exception
    {
        public InvalidIdentifierFormatException(string invalidInput) : base($"Failed to convert '{invalidInput}' value to proper Guid type!") { }
    }
}
