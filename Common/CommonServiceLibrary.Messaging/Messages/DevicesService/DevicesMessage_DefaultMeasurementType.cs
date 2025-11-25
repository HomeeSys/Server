namespace CommonServiceLibrary.Messaging.Messages.DevicesService;

public record DevicesMessage_DefaultMeasurementType(
    int ID,
    string Name,
    string Unit
);