namespace Raports.Application.Messages;

internal class RaportFailed
{
    public DateTime FailedDate { get; set; }
    public string Description { get; set; }
    public DefaultRaportDTO Raport { get; set; }
}
