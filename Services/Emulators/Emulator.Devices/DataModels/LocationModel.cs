namespace Emulator.Devices.DataModels;

internal class LocationModel
{
    public string Name { get; set; } = default!;

    public bool IsEqual(LocationModel other)
    {
        return Name == other.Name;
    }

    public void Update(LocationModel other)
    {
        Name = other.Name;
    }
}
