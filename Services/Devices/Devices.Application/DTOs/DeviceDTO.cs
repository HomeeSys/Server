namespace Devices.Application.DTOs
{
    public record RegisterDeviceDTO(Guid DeviceNumber, string Description);
    public record DeviceDTO(int ID, Guid DeviceNumber, DateTime RegisterDate, string Description, 
        int LocationID, LocationDTO Location, 
        int TimestampConfigurationID, TimestampConfigurationDTO TimestampConfiguration,
        int StatusID, StatusDTO Status);
    public record UpdateDeviceDTO(Guid DeviceNumber, string? Description, int? LocationID, int? TimestampConfigurationID, int? StatusID);
}
