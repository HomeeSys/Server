using Devices.DataTransferObjects;

namespace CommonServiceLibrary.Messaging.Events
{
    public class DeviceStatusChangedMessage : MqMessageBase
    {
        public DefaultDeviceDTO Payload { get; set; }
    }
}
