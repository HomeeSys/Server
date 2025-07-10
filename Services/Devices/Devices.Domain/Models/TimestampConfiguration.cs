namespace Devices.Domain.Models
{
    public class TimestampConfiguration
    {
        public int Id { get; set; }

        /// <summary>
        /// Holds definition when to take measurements. Based on CRON Notation.
        /// </summary>
        public string Cron { get; set; } = default!;
    }
}
