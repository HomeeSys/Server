namespace CommonServiceLibrary.GRPC.Entities;

public class DeviceGRPC
{
    public int ID { get; set; }
    public Guid DeviceNumber { get; set; }
    public DateTime RegisterDate { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }
    public int LocationId { get; set; }
    public LocationGRPC Location { get; set; }
    public int TimestampConfigurationId { get; set; }
    public TimestampConfigurationGRPC TimestampConfiguration { get; set; }
    public int StatusId { get; set; }
    public StatusGRPC Status { get; set; }
}
