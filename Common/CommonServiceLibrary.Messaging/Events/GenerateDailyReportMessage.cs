namespace CommonServiceLibrary.Messaging.Events;

public class GenerateDailyReportMessage : MqMessageBase
{
    public DateTime RaportDate { get; set; }
}
