namespace CommonServiceLibrary.GRPC.Types.Devices;

public record DeviceGRPC(
    int ID,
    string Name,
    Guid DeviceNumber,
    DateTime RegisterDate,
    LocationGRPC Location,
    TimestampGRPC Timestamp,
    ICollection<MeasurementTypeGRPC> MeasurementTypes,
    StatusGRPC Status
);