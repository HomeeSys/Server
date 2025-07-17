namespace CommonServiceLibrary.GRPC.Entities;

public class TimestampConfigurationGRPC
{
    public int Id { get; set; }
    public string Cron { get; set; } = default!;
}
