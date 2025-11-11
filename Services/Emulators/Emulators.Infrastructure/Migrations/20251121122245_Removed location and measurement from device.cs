using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removedlocationandmeasurementfromdevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Locations_LocationID",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_LocationID",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Devices");

            migrationBuilder.AddColumn<int>(
                name: "MeasurementID",
                table: "Devices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_MeasurementID",
                table: "Devices",
                column: "MeasurementID");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Measurements_MeasurementID",
                table: "Devices",
                column: "MeasurementID",
                principalTable: "Measurements",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Measurements_MeasurementID",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_MeasurementID",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "MeasurementID",
                table: "Devices");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_LocationID",
                table: "Devices",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Locations_LocationID",
                table: "Devices",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
