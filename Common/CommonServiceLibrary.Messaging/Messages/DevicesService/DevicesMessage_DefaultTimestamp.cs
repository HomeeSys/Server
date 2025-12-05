namespace CommonServiceLibrary.Messaging.Messages.DevicesService;

public record DevicesMessage_DefaultTimestamp(
    int ID,
    string Cron
);