namespace Devices.Application.Exceptions
{
    public class DuplicateDevice : DuplicateEntityException
    {
        public DuplicateDevice(object key) : base("Device", key) { }
    }
}
