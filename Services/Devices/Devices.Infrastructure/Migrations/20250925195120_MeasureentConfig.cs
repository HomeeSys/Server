using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MeasureentConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasurementConfigId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MeasurementConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<bool>(type: "bit", nullable: false),
                    Humidity = table.Column<bool>(type: "bit", nullable: false),
                    CarbonDioxide = table.Column<bool>(type: "bit", nullable: false),
                    VolatileOrganicCompounds = table.Column<bool>(type: "bit", nullable: false),
                    PM1 = table.Column<bool>(type: "bit", nullable: false),
                    PM25 = table.Column<bool>(type: "bit", nullable: false),
                    PM10 = table.Column<bool>(type: "bit", nullable: false),
                    Formaldehyde = table.Column<bool>(type: "bit", nullable: false),
                    CarbonMonoxide = table.Column<bool>(type: "bit", nullable: false),
                    Ozone = table.Column<bool>(type: "bit", nullable: false),
                    Ammonia = table.Column<bool>(type: "bit", nullable: false),
                    Airflow = table.Column<bool>(type: "bit", nullable: false),
                    AirIonizationLevel = table.Column<bool>(type: "bit", nullable: false),
                    Oxygen = table.Column<bool>(type: "bit", nullable: false),
                    Radon = table.Column<bool>(type: "bit", nullable: false),
                    Illuminance = table.Column<bool>(type: "bit", nullable: false),
                    SoundLevel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementConfigs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementConfigs_DeviceId",
                table: "MeasurementConfigs",
                column: "DeviceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementConfigs");

            migrationBuilder.DropColumn(
                name: "MeasurementConfigId",
                table: "Devices");
        }
    }
}
