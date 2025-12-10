using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Devices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1Initial : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of location, for example: 'Kitchen'..."),
                    Hash = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "For example: 'Air Temperature'"),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "For example: 'ppm'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementTypes", x => x.ID);
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
                name: "Timestamps",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cron = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Timestamp measurement configuration stored in CRON format")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timestamps", x => x.ID);
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
                    TimestampID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Devices_Timestamps_TimestampID",
                        column: x => x.TimestampID,
                        principalTable: "Timestamps",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMeasurementTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMeasurementTypes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeviceMeasurementTypes_Devices_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceMeasurementTypes_MeasurementTypes_MeasurementTypeID",
                        column: x => x.MeasurementTypeID,
                        principalTable: "MeasurementTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMeasurementTypes_DeviceID",
                table: "DeviceMeasurementTypes",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMeasurementTypes_MeasurementTypeID",
                table: "DeviceMeasurementTypes",
                column: "MeasurementTypeID");

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
                name: "IX_Devices_TimestampID",
                table: "Devices",
                column: "TimestampID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMeasurementTypes");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "MeasurementTypes");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Timestamps");
        }
    }
}
