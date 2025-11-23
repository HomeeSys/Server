namespace Emulators.Domain.Models;

public class ChartOffset
{
    public int ID { get; set; }

    /// <summary>
    /// Static offset of whole chart. It can be moved to left or right by time.
    /// </summary>
    public int Time { get; set; }
    /// <summary>
    /// Static offset of whole chart. It can be moved up or down with it's value.
    /// </summary>
    public double Value { get; set; }

    public Location Location { get; set; }
    public int LocationID { get; set; }
    public ChartTemplate ChartTemplate { get; set; }
    public int ChartTemplateID { get; set; }
}
