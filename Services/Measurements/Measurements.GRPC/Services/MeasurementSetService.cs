namespace Measurements.GRPC.Services;

public class MeasurementSetService(MeasurementsDBContext dbContext) : MeasurementService.MeasurementServiceBase
{
    public override async Task<MeasurementSetModel> MeasurementSetByID(MeasurementSetRequest request, ServerCallContext context)
    {
        Guid id = Guid.Parse(request.Id);
        MeasurementSet dbModel = await dbContext.GetMeasurement(id);

        MeasurementSetModel grpcModel = dbModel.Adapt<MeasurementSetModel>();

         return grpcModel;
    }

    public override async Task MeasurementAllSetsByDay(MeasurementSetByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);

        IEnumerable<MeasurementSet> dbMeasurements = await dbContext.GetAllMeasurementsFromDay(day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementAllSetsByWeek(MeasurementSetByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);

        IEnumerable<MeasurementSet> dbMeasurements = await dbContext.GetAllMeasurementsFromWeek(day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementAllSetsByMonth(MeasurementSetByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);

        IEnumerable<MeasurementSet> dbMeasurements = await dbContext.GetAllMeasurementsFromMonth(day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementSetsByDay(MeasurementSetFromDeviceByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        IEnumerable<MeasurementSet> dbMeasurements = await dbContext.GetMeasurementsFromDay(deviceNumber, day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementSetsByWeek(MeasurementSetFromDeviceByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        IEnumerable<MeasurementSet> dbMeasurements = await dbContext.GetMeasurementsFromWeek(deviceNumber, day);

        IEnumerable<MeasurementSetModel> grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementSetModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementSetsByMonth(MeasurementSetFromDeviceByDateRequest request, IServerStreamWriter<MeasurementSetModel> responseStream, ServerCallContext context)
    {
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        IEnumerable<MeasurementSet> dbMeasurements = await dbContext.GetMeasurementsFromMonth(deviceNumber, day);

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
        if(ok == false)
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
