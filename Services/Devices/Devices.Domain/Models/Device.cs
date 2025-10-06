namespace Devices.Domain.Models
{
    public class Device
    {
        /// <summary>
        /// ID for database.
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        /// <summary>
        /// Unique device number that build into DEVICE.
        /// </summary>
        public Guid DeviceNumber { get; set; }

        /// <summary>
        /// Device registration date.
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// Foreign key to `Location` table.
        /// </summary>
        public int LocationId { get; set; }
        public Location Location { get; set; } = default!;

        /// <summary>
        /// Foreign key to `TimestampConfiguration` table.
        /// </summary>
        public int TimestampConfigurationId { get; set; }
        public TimestampConfiguration TimestampConfiguration { get; set; } = default!;

        public int StatusId { get; set; }
        public Status Status { get; set; } = default!;

        public int MeasurementConfigId { get; set; }
        public MeasurementConfig MeasurementConfiguration { get; set; } = default!;
    }
}
