namespace CommonServiceLibrary.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity, object key) : base($"\"{entity}\" with \"{key}\" not found!")
        {
            
        }
    }
}
