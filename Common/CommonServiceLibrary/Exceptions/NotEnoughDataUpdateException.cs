namespace CommonServiceLibrary.Exceptions
{
    public class NotEnoughDataUpdateException : Exception
    {
        public NotEnoughDataUpdateException(string entity) : base($"Not enough data provided to update \"{entity}\"")
        {
            
        }
    }
}
