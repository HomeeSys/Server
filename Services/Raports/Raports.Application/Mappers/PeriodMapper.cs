namespace Raports.Application.Mappers;

internal class PeriodMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        TypeAdapterConfig<Period, DefaultPeriodDTO>
            .NewConfig()
            .Map(x => x.ID, y => y.ID)
            .Map(x => x.Name, y => y.Name);
    }
}
