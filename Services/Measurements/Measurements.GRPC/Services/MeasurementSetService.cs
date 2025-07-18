namespace Measurements.GRPC.Services;

public class MeasurementSetService : MeasurementService.MeasurementServiceBase
{
    private readonly ILogger<MeasurementSetService> _logger;
    private readonly MeasurementsDBContext _dbContext;
    public MeasurementSetService(MeasurementsDBContext dbContext, ILogger<MeasurementSetService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

        TypeAdapterConfig<Measurement, MeasurementModel>
            .NewConfig()
            .Map(dest => dest.Value, src => src.Value)
            .Map(dest => dest.Unit, src => src.Unit);

        TypeAdapterConfig<MeasurementSet, MeasurementSetModel>
            .NewConfig()
            .Map(dest => dest.Co, src => src.CO)
            .Map(dest => dest.Voc, src => src.VOC)
            .Map(dest => dest.ParticulateMatter1, src => src.ParticulateMatter1)
            .Map(dest => dest.ParticulateMatter2V5, src => src.ParticulateMatter2v5)
            .Map(dest => dest.ParticulateMatter10, src => src.ParticulateMatter10)
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Co2, src => src.CO2)
            .Map(dest => dest.RegisterDate, src => src.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    public override async Task<MeasurementSetModel> MeasurementSetByID(MeasurementSetRequest request, ServerCallContext context)
    {
        Guid id = Guid.Parse(request.Id);
        MeasurementSet dbModel = await _dbContext.GetMeasurement(id);

        MeasurementSetModel grpcModel = dbModel.Adapt<MeasurementSetModel>();

        return grpcModel;
    }

    public override async Task MeasurementAllSetsByDay(MeasurementSetByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);

        IEnumerable<MeasurementSet> dbMeasurements = await _dbContext.GetAllMeasurementsFromDay(day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementAllSetsByWeek(MeasurementSetByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);

        IEnumerable<MeasurementSet> dbMeasurements = await _dbContext.GetAllMeasurementsFromWeek(day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementAllSetsByMonth(MeasurementSetByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);

        IEnumerable<MeasurementSet> dbMeasurements = await _dbContext.GetAllMeasurementsFromMonth(day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementSetsByDay(MeasurementSetFromDeviceByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        IEnumerable<MeasurementSet> dbMeasurements = await _dbContext.GetMeasurementsFromDay(deviceNumber, day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementSetsByWeek(MeasurementSetFromDeviceByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        IEnumerable<MeasurementSet> dbMeasurements = await _dbContext.GetMeasurementsFromWeek(deviceNumber, day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementSetsByMonth(MeasurementSetFromDeviceByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        IEnumerable<MeasurementSet> dbMeasurements = await _dbContext.GetMeasurementsFromMonth(deviceNumber, day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    private async Task SendData(IEnumerable<MeasurementSetModel> Data, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        foreach (MeasurementSetModel model in Data)
        {
            await responseStream.WriteAsync(model, context.CancellationToken);
        }
    }

    private DateTime ParseDateTime(string dateTime)
    {
        DateTime date = DateTime.MinValue;
        bool ok = DateTime.TryParse(dateTime, out date);
        if (ok == false)
        {
            throw new InvalidDateFormatException(dateTime);
        }

        return date;
    }

    private Guid ParseIdentifier(string identifier)
    {
        Guid id = Guid.Empty;
        bool ok = Guid.TryParse(identifier, out id);
        if (ok == false)
        {
            throw new InvalidIdentifierFormatException(identifier);
        }

        return id;
    }
}
