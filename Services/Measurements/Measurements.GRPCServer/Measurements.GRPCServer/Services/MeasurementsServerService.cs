namespace Measurements.GRPCServer.Services;

public class MeasurementsServerService(ILogger<MeasurementsServerService> logger, MeasurementsDBContext dbcontext) : MeasurementService.MeasurementServiceBase
{
    public override async Task<MeasurementGrpcModel> MeasurementByID(MeasurementRequest request, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        Guid id = Guid.Parse(request.Id);
        var dbModel = await dbcontext.GetMeasurement(id);

        var grpcModel = dbModel.Adapt<MeasurementGrpcModel>();

        return grpcModel;
    }

    public override async Task MeasurementsAllByDay(MeasurementByDateRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        DateTime day = ParseDateTime(request.Date);

        var dbMeasurements = await dbcontext.GetAllMeasurementsFromDay(day);

        var grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementGrpcModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementsAllByWeek(MeasurementByDateRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        DateTime day = ParseDateTime(request.Date);

        var dbMeasurements = await dbcontext.GetAllMeasurementsFromWeek(day);

        var grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementGrpcModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementsAllByMonth(MeasurementByDateRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        DateTime day = ParseDateTime(request.Date);

        var dbMeasurements = await dbcontext.GetAllMeasurementsFromMonth(day);

        var grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementGrpcModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementsByDay(MeasurementFromDeviceByDateRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        var dbMeasurements = await dbcontext.GetMeasurementsFromDay(deviceNumber, day);

        var grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementGrpcModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementsByWeek(MeasurementFromDeviceByDateRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        var dbMeasurements = await dbcontext.GetMeasurementsFromWeek(deviceNumber, day);

        var grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementGrpcModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    public override async Task MeasurementsByMonth(MeasurementFromDeviceByDateRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        throw new NotImplementedException("TODO: We have changed measurement data definition so we have to rewrite that!");
        DateTime day = ParseDateTime(request.Date);
        Guid deviceNumber = ParseIdentifier(request.DeviceNumber);

        var dbMeasurements = await dbcontext.GetMeasurementsFromMonth(deviceNumber, day);

        var grpsMeasurements = dbMeasurements.Adapt<IEnumerable<MeasurementGrpcModel>>();

        await SendData(grpsMeasurements, responseStream, context);
    }

    private async Task SendData(IEnumerable<MeasurementGrpcModel> Data, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        foreach (var model in Data)
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
            throw new Exception(dateTime);
        }

        return date;
    }

    private Guid ParseIdentifier(string identifier)
    {
        Guid id = Guid.Empty;
        bool ok = Guid.TryParse(identifier, out id);
        if (ok == false)
        {
            throw new Exception(identifier);
        }

        return id;
    }
}