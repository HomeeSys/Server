namespace CommonServiceLibrary.Messaging.Events;

public class MqMessageBase
{
    public Guid ID { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; } = DateTime.Now;
}