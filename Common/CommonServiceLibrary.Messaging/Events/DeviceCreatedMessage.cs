using Devices.DataTransferObjects;

namespace CommonServiceLibrary.Messaging.Events
{
    public class DeviceCreatedMessage : MqMessageBase
    {
        public DefaultDeviceDTO NewDevice { get; set; }
    }
}