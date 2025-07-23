namespace CommonServiceLibrary.Messaging.Events;

public class EnqueueDailyRaportGenerationMessage : MqMessageBase
{
    public DateTime RaportDate { get; set; }
}
