namespace CommonServiceLibrary.Messaging.TopicMessages.Devices;

public class DeviceGenerateMeasurement
{
    /// <summary>
    /// Describes date and time for measurement creation.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Description of device that will generate measurement.
    /// </summary>
    public DevicesMessage_DefaultDevice Device { get; set; }
}
