namespace Raports.Infrastructure.Generators;

public class MeasurementPacketGenerator
{
    //public readonly MeasurementsClientGRPC _measurementsGrpcClient;
    //public readonly DevicesClientGRPC _devicesGrpcClient;
    //private readonly AiResponseClientGRPC _aiGrpcClient;
    public readonly IConfiguration _configuration;
    public MeasurementPacketGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        //_measurementsGrpcClient = new MeasurementsClientGRPC(configuration);
        //_devicesGrpcClient = new DevicesClientGRPC(configuration);
        //_aiGrpcClient = new AiResponseClientGRPC(configuration);
    }

    public async Task<IEnumerable<MeasurementPacket>> ProcessDailyDataForReport(DateTime date)
    {
        //IEnumerable<MeasurementSetGRPC> measurements = await _measurementsGrpcClient.GetAllMeasurementsFromDay(date);
        //IEnumerable<DeviceGRPC> devices = await _devicesGrpcClient.GetAllDevices();

        //  This steps allow to convert data from `Measurements` and `Devices` collection into usable packets of
        //  usable data. For example, `Temperatures` package that contains datasets for each of available locations.
        List<MeasurementPacket> measurementsPackets = MeasurementPacketFiller.AgregateDailyData(null, null);

        var tasks = new List<Task>();

        foreach (MeasurementPacket packet in measurementsPackets)
        {
            MeasurementAnalysisModel model = packet.ToAnalysisModel();
            string promptData = model.ToJson();

            //AiResponseGRPC response = await _aiGrpcClient.GenerateDescriptionForDailyRaport(promptData);

            packet.Description = null;
        }

        return measurementsPackets;
    }
}
