namespace Devices.Application.Exceptions
{
    internal class StatusNotFoundException : EntityNotFoundException
    {
        public StatusNotFoundException(object key) : base("Status", key)
        {
        }
    }
}
