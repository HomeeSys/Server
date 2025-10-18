using Devices.DataTransferObjects;

namespace CommonServiceLibrary.Messaging.Events
{
    public class DeviceDeletedMessage : MqMessageBase
    {
        public DefaultDeviceDTO DeletedDevice { get; set; }
    }
}
