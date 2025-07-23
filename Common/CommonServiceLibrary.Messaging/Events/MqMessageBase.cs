namespace CommonServiceLibrary.Messaging.Events;

public class MqMessageBase
{
    public Guid ID { get; set; }
    public DateTime Created { get; set; }
}