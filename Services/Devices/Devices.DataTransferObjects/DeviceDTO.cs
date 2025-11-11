namespace Devices.DataTransferObjects;

public record DefaultDeviceDTO(
    int ID,
    string Name,
    Guid DeviceNumber,
    DateTime RegisterDate,
    LocationDTO Location,
    TimestampDTO Timestamp,
    ICollection<DefaultMeasurementTypeDTO> MeasurementTypes,
    StatusDTO Status
);