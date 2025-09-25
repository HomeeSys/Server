using CommonServiceLibrary.Messaging.Events;

namespace Devices.Application.Messages;

public class DeviceStatusChangedMessage : MqMessageBase
{
    public int DeviceID { get; set; }
    public int StatusID { get; set; }
    public string StatusType { get; set; }
}
