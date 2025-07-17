namespace CommonServiceLibrary.GRPC.Client
{
    public class CommonClientGRPC
    {
        public DevicesClientGRPC Devices;

        public CommonClientGRPC(IConfiguration configuration)
        {
            Devices = new DevicesClientGRPC(configuration);
        }
    }
}
