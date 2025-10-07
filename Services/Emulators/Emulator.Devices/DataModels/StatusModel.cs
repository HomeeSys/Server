namespace Emulator.Devices.DataModels;

internal class StatusModel
{
    public string Type { get; set; } = default!;
    public bool IsEqual(StatusModel other)
    {
        return Type == other.Type;
    }

    public void Update(StatusModel other)
    {
        Type = other.Type;
    }
}
