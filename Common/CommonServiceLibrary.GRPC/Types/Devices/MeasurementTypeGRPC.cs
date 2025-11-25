namespace CommonServiceLibrary.GRPC.Types.Devices;

public record MeasurementTypeGRPC(
    int ID,
    string Name,
    string Unit
);