namespace CommonServiceLibrary.GRPC.Types.Devices;

public record LocationGRPC(
    int ID,
    string Name,
    Guid Hash
);