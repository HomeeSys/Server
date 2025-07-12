namespace Devices.Domain.Models
{
    public class Location
    {
        /// <summary>
        /// Index for database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Location name, for example: `Kitechen`, `Living room`, `Attic`, etc...
        /// </summary>
        public string Name { get; set; } = default!;
    }
}