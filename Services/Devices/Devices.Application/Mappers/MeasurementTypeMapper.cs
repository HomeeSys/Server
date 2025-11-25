namespace Devices.Application.Mappers;

internal class MeasurementTypeMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<MeasurementType, DevicesMessage_DefaultMeasurementType>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<DevicesMessage_DefaultMeasurementType, MeasurementType>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.Name, y => y.Name);

        //  --------------- Mappings with messaging types ---------------
        TypeAdapterConfig<DefaultMeasurementTypeDTO, DevicesMessage_DefaultMeasurementType>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.Name, y => y.Name);

        TypeAdapterConfig<DevicesMessage_DefaultMeasurementType, DefaultMeasurementTypeDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.Name, y => y.Name);

        //  --------------- Mappings with dtos types ---------------
        TypeAdapterConfig<DefaultMeasurementTypeDTO, MeasurementType>
            .NewConfig()
            .Map(x => x.Name, y => y.Name)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.ID, y => y.ID);

        TypeAdapterConfig<MeasurementType, DefaultMeasurementTypeDTO>
            .NewConfig()
            .Map(x => x.Name, y => y.Name)
            .Map(x => x.Unit, y => y.Unit)
            .Map(x => x.ID, y => y.ID);
    }
}
