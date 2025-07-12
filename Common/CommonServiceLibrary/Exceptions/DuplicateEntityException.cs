namespace CommonServiceLibrary.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string entity, object key) : base($"\"{entity}\" with such \"{key}\" already exists")
        {
            
        }
    }
}
