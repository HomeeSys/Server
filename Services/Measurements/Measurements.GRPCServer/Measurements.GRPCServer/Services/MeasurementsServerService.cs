using Microsoft.Azure.Cosmos;

namespace Measurements.GRPCServer.Services;

public class MeasurementsServerService(ILogger<MeasurementsServerService> logger, Container database) : MeasurementService.MeasurementServiceBase
{
    public override async Task MeasurementsQuery(MeasurementsQueryRequest request, IServerStreamWriter<MeasurementGrpcModel> responseStream, ServerCallContext context)
    {
        var orederedQuery = database.GetItemLinqQueryable<Measurement>(allowSynchronousQueryExecution: true);
        var query = orederedQuery.AsQueryable();

        //  Device numbers
        if (request.DeviceNumbers is not null && request.DeviceNumbers.Count != 0)
        {
            var deviceNumbers = request.DeviceNumbers.Select(x => Guid.Parse(x)).ToList();

            query = query.Where(x => deviceNumbers.Contains(x.DeviceNumber));
        }

        //  Locations
        if (request.LocationHashes is not null && request.LocationHashes.Count != 0)
        {
            var locationHashes = request.LocationHashes.Select(x => Guid.Parse(x)).ToList();

            query = query.Where(x => locationHashes.Contains(x.LocationHash));
        }

        if (request.HasStartDate && request.StartDate is not null && request.StartDate != string.Empty)
        {
            var startDate = DateTime.Parse(request.StartDate);
            query = query.Where(x => x.MeasurementCaptureDate >= startDate);
        }

        if (request.HasEndDate && request.EndDate is not null && request.EndDate != string.Empty)
        {
            var endDate = DateTime.Parse(request.EndDate).AddTicks(1);
            query = query.Where(x => x.MeasurementCaptureDate <= endDate);
        }

        //  Order
        if (request.SortOrder is not null)
        {
            if (request.SortOrder == "asc")
            {
                query = query.OrderBy(x => x.MeasurementCaptureDate);
            }
            else
            {
                query = query.OrderByDescending(x => x.MeasurementCaptureDate);
            }
        }

        var queryResult = query.ToList();

        var dtoResponse = queryResult.Adapt<IEnumerable<MeasurementGrpcModel>>();
        var alternativeResp = queryResult.Select(x => new MeasurementGrpcModel()
        {
            Id = x.ID.ToString(),
            DeviceNumber = x.DeviceNumber.ToString(),
            MeasurementCaptureDate = x.MeasurementCaptureDate.ToString("o"),
            LocationHash = x.LocationHash.ToString(),

            Temperature = x.Temperature is not null ? (double)x.Temperature : double.MinValue,
            Humidity = x.Humidity is not null ? (double)x.Humidity : double.MinValue,
            Co2 = x.CarbonDioxide is not null ? (double)x.CarbonDioxide : double.MinValue,
            Voc = x.VolatileOrganicCompounds is not null ? (double)x.VolatileOrganicCompounds : double.MinValue,
            ParticulateMatter1 = x.ParticulateMatter1 is not null ? (double)x.ParticulateMatter1 : double.MinValue,
            ParticulateMatter2V5 = x.ParticulateMatter2v5 is not null ? (double)x.ParticulateMatter2v5 : double.MinValue,
            ParticulateMatter10 = x.ParticulateMatter10 is not null ? (double)x.ParticulateMatter10 : double.MinValue,
            Formaldehyde = x.Formaldehyde is not null ? (double)x.Formaldehyde : double.MinValue,
            Co = x.CarbonMonoxide is not null ? (double)x.CarbonMonoxide : double.MinValue,
            O3 = x.Ozone is not null ? (double)x.Ozone : double.MinValue,
            Ammonia = x.Ammonia is not null ? (double)x.Ammonia : double.MinValue,
            Airflow = x.Airflow is not null ? (double)x.Airflow : double.MinValue,
            AirIonizationLevel = x.AirIonizationLevel is not null ? (double)x.AirIonizationLevel : double.MinValue,
            O2 = x.Oxygen is not null ? (double)x.Oxygen : double.MinValue,
            Radon = x.Radon is not null ? (double)x.Radon : double.MinValue,
            Illuminance = x.Illuminance is not null ? (double)x.Illuminance : double.MinValue,
            SoundLevel = x.SoundLevel is not null ? (double)x.SoundLevel : double.MinValue,
        });

        await SendData(alternativeResp, responseStream, context);
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