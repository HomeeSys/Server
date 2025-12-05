namespace CommonServiceLibrary.Messaging.Messages.DevicesService;

public record DevicesMessage_DefaultDevice(
    int ID,
    string Name,
    Guid DeviceNumber,
    DateTime RegisterDate,
    DevicesMessage_DefaultLocation Location,
    DevicesMessage_DefaultTimestamp Timestamp,
    ICollection<DevicesMessage_DefaultMeasurementType> MeasurementTypes,
    DevicesMessage_DefaultStatus Status
);