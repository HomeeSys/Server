namespace Raports.Infrastructure.Generators;

public class MeasurementPacketGenerator
{
    public readonly MeasurementsClientGRPC _measurementsGrpcClient;
    public readonly DevicesClientGRPC _devicesGrpcClient;
    public readonly IConfiguration _configuration;
    public MeasurementPacketGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _measurementsGrpcClient = new MeasurementsClientGRPC(configuration);
        _devicesGrpcClient = new DevicesClientGRPC(configuration);
    }

    public async Task<IEnumerable<MeasurementPacket>> ProcessDailyDataForReport(DateTime date)
    {
        IEnumerable<MeasurementSetGRPC> measurements = await _measurementsGrpcClient.GetAllMeasurementsFromDay(date);
        IEnumerable<DeviceGRPC> devices = await _devicesGrpcClient.GetAllDevices();

        //  This steps allow to convert data from `Measurements` and `Devices` collection into usable packets of
        //  usable data. For example, `Temperatures` package that contains datasets for each of available locations.
        List<MeasurementPacket> measurementsPackets = MeasurementPacketFiller.AgregateDailyData(devices, measurements);

        //  It is time to as LLM about description for each of data packet.
        foreach (MeasurementPacket packet in measurementsPackets)
        {
            packet.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        }

        return measurementsPackets;
    }
}
