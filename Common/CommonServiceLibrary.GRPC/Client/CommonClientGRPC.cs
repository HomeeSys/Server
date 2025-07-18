namespace CommonServiceLibrary.GRPC.Client
{
    public class CommonClientGRPC
    {
        public DevicesClientGRPC Devices;
        public MeasurementsClientGRPC Measurements;

        public CommonClientGRPC(IConfiguration configuration)
        {
            Devices = new DevicesClientGRPC(configuration);
            Measurements = new MeasurementsClientGRPC(configuration);
        }
    }
}
