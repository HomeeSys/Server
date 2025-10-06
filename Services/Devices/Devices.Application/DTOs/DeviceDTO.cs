﻿namespace Devices.Application.DTOs
{
    public record RegisterDeviceDTO(Guid DeviceNumber, string Description);
    public record DeviceDTO(int ID, string Name, Guid DeviceNumber, DateTime RegisterDate,
        int LocationID, LocationDTO Location,
        int TimestampConfigurationID, TimestampConfigurationDTO TimestampConfiguration,
        int MeasurementConfigID, MeasurementConfigDTO MeasurementConfig,
        int StatusID, StatusDTO Status);

    public record DefaultDeviceDTO(
        int ID,
        string Name,
        Guid DeviceNumber,
        DateTime RegisterDate,
        LocationDTO Location,
        TimestampConfigurationDTO TimestampConfiguration,
        DefaultMeasurementConfigurationDTO MeasurementConfiguration,
        StatusDTO Status
    );

    public record UpdateDeviceDTO(Guid DeviceNumber, string? Name, int? LocationID, int? TimestampConfigurationID, int? StatusID);
    public record UpdateDeviceStatusDTO(int StatusID, string? StatusType);
}
