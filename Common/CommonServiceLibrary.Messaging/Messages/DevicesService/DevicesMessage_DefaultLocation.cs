namespace CommonServiceLibrary.Messaging.Messages.DevicesService;

public record DevicesMessage_DefaultLocation(
    int ID,
    string Name,
    Guid Hash
);