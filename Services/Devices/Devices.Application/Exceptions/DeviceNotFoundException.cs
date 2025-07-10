namespace Devices.Application.Exceptions
{
    internal class DeviceNotFoundException : EntityNotFoundException
    {
        public DeviceNotFoundException(object key) : base("Device", key)
        {
        }
    }
}
