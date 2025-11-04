namespace Raports.Infrastructure.DataFilles;

public static class MeasurementPacketFiller
{
    /// <summary>
    /// Collect daily data from `Measurements` and `Devices`.
    /// </summary>
    /// <param name="Devices"></param>
    /// <param name="Measurements"></param>
    /// <returns></returns>
    public static List<MeasurementPacket> AgregateDailyData(IEnumerable<DeviceGRPC> Devices, IEnumerable<MeasurementSetGRPC> Measurements)
    {
        var temps = from measurement in Measurements
                    select new
                    {
                        DeviceNumber = measurement.DeviceNumber,
                        MeasurementDate = new DateTime(measurement.RegisterDate.Year, measurement.RegisterDate.Month, measurement.RegisterDate.Day, measurement.RegisterDate.Hour, 0, 0),
                        Temperature = measurement.Temperature.Value,
                        Humidity = measurement.Humidity.Value,
                        Co2 = measurement.CO2.Value,
                        Voc = measurement.VOC.Value,
                        ParticulateMatter1 = measurement.PartuculateMatter1.Value,
                        ParticulateMatter2_5 = measurement.PartuculateMatter2v5.Value,
                        ParticulateMatter10 = measurement.PartuculateMatter10.Value,
                        Formaldehyde = measurement.Formaldehyde.Value,
                        Co = measurement.CO.Value,
                        O3 = measurement.O3.Value,
                        Ammonia = measurement.Ammonia.Value,
                        Airflow = measurement.Airflow.Value,
                        AirIonizationLevel = measurement.AirIonizationLevel.Value,
                        O2 = measurement.O2.Value,
                        Radon = measurement.Radon.Value,
                        Illuminance = measurement.Illuminance.Value,
                        SoundLevel = measurement.SoundLevel.Value,
                    };

        var tempsGroupedByDevice = from temp in temps group (temp) by temp.DeviceNumber;

        var itit = from item in tempsGroupedByDevice
                   join device in Devices
                   on item.Key equals device.DeviceNumber
                   select (device, item.ToList());

        var averagedByLocation = itit
            .GroupBy(entry => entry.device.LocationID)
            .Select(locationGroup => new
            {
                LocationId = locationGroup.Key,
                HourlyAverages = locationGroup
                    .SelectMany(entry => entry.Item2)
                    .GroupBy(m => m.MeasurementDate) // already floored to the hour
                    .Select(g => new
                    {
                        Date = g.Key,
                        Temperature = g.Average(x => x.Temperature),
                        Humidity = g.Average(x => x.Humidity),
                        CO2 = g.Average(x => x.Co2),
                        VOC = g.Average(x => x.Voc),
                        PM1 = g.Average(x => x.ParticulateMatter1),
                        PM2_5 = g.Average(x => x.ParticulateMatter2_5),
                        PM10 = g.Average(x => x.ParticulateMatter10),
                        Formaldehyde = g.Average(x => x.Formaldehyde),
                        CO = g.Average(x => x.Co),
                        O3 = g.Average(x => x.O3),
                        Ammonia = g.Average(x => x.Ammonia),
                        Airflow = g.Average(x => x.Airflow),
                        AirIonizationLevel = g.Average(x => x.AirIonizationLevel),
                        O2 = g.Average(x => x.O2),
                        Radon = g.Average(x => x.Radon),
                        Illuminance = g.Average(x => x.Illuminance),
                        SoundLevel = g.Average(x => x.SoundLevel)
                    })
                    .OrderBy(x => x.Date)
                    .ToArray()
            })
            .ToArray();

        var locs = Devices.Select(x => x.Location).Distinct().ToArray();

        DateTime[] dates = averagedByLocation.FirstOrDefault().HourlyAverages.Select(x => x.Date).ToArray();

        List<MeasurementData> TemperatureMesData = new List<MeasurementData>();
        List<MeasurementData> HumidityMesData = new List<MeasurementData>();
        List<MeasurementData> CO2MesData = new List<MeasurementData>();
        List<MeasurementData> VOCMesData = new List<MeasurementData>();
        List<MeasurementData> PartuculateMatter1MesData = new List<MeasurementData>();
        List<MeasurementData> PartuculateMatter2v5MesData = new List<MeasurementData>();
        List<MeasurementData> PartuculateMatter10MesData = new List<MeasurementData>();
        List<MeasurementData> FormaldehydeMesData = new List<MeasurementData>();
        List<MeasurementData> COMesData = new List<MeasurementData>();
        List<MeasurementData> O3MesData = new List<MeasurementData>();
        List<MeasurementData> AmmoniaMesData = new List<MeasurementData>();
        List<MeasurementData> AirflowMesData = new List<MeasurementData>();
        List<MeasurementData> AirIonizationLevelMesData = new List<MeasurementData>();
        List<MeasurementData> O2LevelMesData = new List<MeasurementData>();
        List<MeasurementData> RadonLevelMesData = new List<MeasurementData>();
        List<MeasurementData> IlluminanceLevelMesData = new List<MeasurementData>();
        List<MeasurementData> SoundLevelLevelMesData = new List<MeasurementData>();

        foreach (var item in averagedByLocation)
        {
            string label = locs.Where(x => x.ID == item.LocationId).FirstOrDefault().Name;

            List<double> tempsTMP = new List<double>();
            List<double> humiTMP = new List<double>();
            List<double> co2TMP = new List<double>();
            List<double> vocTMP = new List<double>();
            List<double> pm1TMP = new List<double>();
            List<double> pm2_5TMP = new List<double>();
            List<double> pm10TMP = new List<double>();
            List<double> formaldehydeTMP = new List<double>();
            List<double> coTMP = new List<double>();
            List<double> o3TMP = new List<double>();
            List<double> ammoniaTMP = new List<double>();
            List<double> airflowTMP = new List<double>();
            List<double> airIonizationLevelTMP = new List<double>();
            List<double> o2TMP = new List<double>();
            List<double> radonTMP = new List<double>();
            List<double> illuminanceTMP = new List<double>();
            List<double> soundLevelTMP = new List<double>();

            foreach (var dateItem in item.HourlyAverages)
            {
                var avregePerHour = dateItem;

                tempsTMP.Add(avregePerHour.Temperature);
                humiTMP.Add(avregePerHour.Humidity);
                co2TMP.Add(avregePerHour.CO2);
                vocTMP.Add(avregePerHour.VOC);
                pm1TMP.Add(avregePerHour.PM1);
                pm2_5TMP.Add(avregePerHour.PM2_5);
                pm10TMP.Add(avregePerHour.PM10);
                formaldehydeTMP.Add(avregePerHour.Formaldehyde);
                coTMP.Add(avregePerHour.CO);
                o3TMP.Add(avregePerHour.O3);
                ammoniaTMP.Add(avregePerHour.Ammonia);
                airflowTMP.Add(avregePerHour.Airflow);
                airIonizationLevelTMP.Add(avregePerHour.AirIonizationLevel);
                o2TMP.Add(avregePerHour.O2);
                radonTMP.Add(avregePerHour.Radon);
                illuminanceTMP.Add(avregePerHour.Illuminance);
                soundLevelTMP.Add(avregePerHour.SoundLevel);
            }

            TemperatureMesData.Add(new MeasurementData(label, tempsTMP.ToArray()));
            HumidityMesData.Add(new MeasurementData(label, humiTMP.ToArray()));
            CO2MesData.Add(new MeasurementData(label, co2TMP.ToArray()));
            VOCMesData.Add(new MeasurementData(label, vocTMP.ToArray()));
            PartuculateMatter1MesData.Add(new MeasurementData(label, pm1TMP.ToArray()));
            PartuculateMatter2v5MesData.Add(new MeasurementData(label, pm2_5TMP.ToArray()));
            PartuculateMatter10MesData.Add(new MeasurementData(label, pm10TMP.ToArray()));
            FormaldehydeMesData.Add(new MeasurementData(label, formaldehydeTMP.ToArray()));
            COMesData.Add(new MeasurementData(label, coTMP.ToArray()));
            O3MesData.Add(new MeasurementData(label, o3TMP.ToArray()));
            AmmoniaMesData.Add(new MeasurementData(label, ammoniaTMP.ToArray()));
            AirflowMesData.Add(new MeasurementData(label, airflowTMP.ToArray()));
            AirIonizationLevelMesData.Add(new MeasurementData(label, airIonizationLevelTMP.ToArray()));
            O2LevelMesData.Add(new MeasurementData(label, o2TMP.ToArray()));
            RadonLevelMesData.Add(new MeasurementData(label, radonTMP.ToArray()));
            IlluminanceLevelMesData.Add(new MeasurementData(label, illuminanceTMP.ToArray()));
            SoundLevelLevelMesData.Add(new MeasurementData(label, soundLevelTMP.ToArray()));
        }

        MeasurementPacket temperaturesPacket = new MeasurementPacket(dates, TemperatureMesData, 1, 1, 1, "Temperatures", "");
        MeasurementPacket humidityPacket = new MeasurementPacket(dates, HumidityMesData, 1, 1, 1, "Humidity", "");
        MeasurementPacket co2Packet = new MeasurementPacket(dates, CO2MesData, 1, 1, 1, "Carbon dioxide", "");
        MeasurementPacket vocPacket = new MeasurementPacket(dates, VOCMesData, 1, 1, 1, "Volatile organic compound", "");
        MeasurementPacket pm1Packet = new MeasurementPacket(dates, PartuculateMatter1MesData, 1, 1, 1, "Particulate matter 1", "");
        MeasurementPacket pm25Packet = new MeasurementPacket(dates, PartuculateMatter2v5MesData, 1, 1, 1, "Particulate matter 2.5", "");
        MeasurementPacket om10Packet = new MeasurementPacket(dates, PartuculateMatter10MesData, 1, 1, 1, "Particulate matter 10", "");
        MeasurementPacket formalPacket = new MeasurementPacket(dates, FormaldehydeMesData, 1, 1, 1, "Formaldehyde", "");
        MeasurementPacket coPacket = new MeasurementPacket(dates, COMesData, 1, 1, 1, "Carbon monoxide", "");
        MeasurementPacket o3Packet = new MeasurementPacket(dates, O3MesData, 1, 1, 1, "Ozone", "");
        MeasurementPacket ammoniaPacket = new MeasurementPacket(dates, AmmoniaMesData, 1, 1, 1, "Ammonia", "");
        MeasurementPacket airflowPacket = new MeasurementPacket(dates, AirflowMesData, 1, 1, 1, "Airflow", "");
        MeasurementPacket airIonizationlvlPacket = new MeasurementPacket(dates, AirIonizationLevelMesData, 1, 1, 1, "Air ionization level", "");
        MeasurementPacket o2Packet = new MeasurementPacket(dates, O2LevelMesData, 1, 1, 1, "Oxygen", "");
        MeasurementPacket radonPacket = new MeasurementPacket(dates, RadonLevelMesData, 1, 1, 1, "Radon", "");
        MeasurementPacket illuminancePacket = new MeasurementPacket(dates, IlluminanceLevelMesData, 1, 1, 1, "Illuminance", "");
        MeasurementPacket soundLevelPacket = new MeasurementPacket(dates, SoundLevelLevelMesData, 1, 1, 1, "Sound level", "");

        List<MeasurementPacket> packetsList =
        [
            temperaturesPacket,
            humidityPacket,
            co2Packet,
            vocPacket,
            pm1Packet,
            pm25Packet,
            om10Packet,
            formalPacket,
            coPacket,
            o3Packet,
            ammoniaPacket,
            airflowPacket,
            airIonizationlvlPacket,
            o2Packet,
            radonPacket,
            illuminancePacket,
            soundLevelPacket,
        ];

        return packetsList;
    }
}
