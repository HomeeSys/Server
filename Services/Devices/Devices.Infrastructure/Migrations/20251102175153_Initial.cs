using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of location, for example: 'Kitchen'...")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TimestampConfigurations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cron = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Timestamp measurement configuration stored in CRON format")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimestampConfigurations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "This is unique name for device"),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    TimestampConfigurationID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    MeasurementConfigurationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Devices_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devices_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Devices_TimestampConfigurations_TimestampConfigurationID",
                        column: x => x.TimestampConfigurationID,
                        principalTable: "TimestampConfigurations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementConfigurations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MeasurementConfigurations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MeasurementConfigurations_Devices_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceNumber",
                table: "Devices",
                column: "DeviceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_LocationID",
                table: "Devices",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_StatusID",
                table: "Devices",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_TimestampConfigurationID",
                table: "Devices",
                column: "TimestampConfigurationID");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementConfigurations_DeviceID",
                table: "MeasurementConfigurations",
                column: "DeviceID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeasurementConfigurations");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "TimestampConfigurations");
        }
    }
}
