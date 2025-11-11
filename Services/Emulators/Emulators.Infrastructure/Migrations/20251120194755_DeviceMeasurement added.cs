using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeviceMeasurementadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceMeasurements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    MeasurementID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMeasurements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeviceMeasurements_Devices_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceMeasurements_Measurements_MeasurementID",
                        column: x => x.MeasurementID,
                        principalTable: "Measurements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMeasurements_DeviceID",
                table: "DeviceMeasurements",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMeasurements_MeasurementID",
                table: "DeviceMeasurements",
                column: "MeasurementID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMeasurements");
        }
    }
}
