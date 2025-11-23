using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "This is unique name for device")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Biases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    MeasurementTypeID = table.Column<int>(type: "int", nullable: false),
                    TimeOffset = table.Column<int>(type: "int", nullable: false),
                    ValueOffset = table.Column<double>(type: "float", nullable: false),
                    ValueSpread = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biases", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Biases_Devices_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "Devices",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biases_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biases_MeasurementTypes_MeasurementTypeID",
                        column: x => x.MeasurementTypeID,
                        principalTable: "MeasurementTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementTypeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartTemplates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChartTemplates_MeasurementTypes_MeasurementTypeID",
                        column: x => x.MeasurementTypeID,
                        principalTable: "MeasurementTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Samples_ChartTemplates_ChartTemplateID",
                        column: x => x.ChartTemplateID,
                        principalTable: "ChartTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biases_DeviceID",
                table: "Biases",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Biases_LocationID",
                table: "Biases",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Biases_MeasurementTypeID",
                table: "Biases",
                column: "MeasurementTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_MeasurementTypeID",
                table: "ChartTemplates",
                column: "MeasurementTypeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceNumber",
                table: "Devices",
                column: "DeviceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Samples_ChartTemplateID",
                table: "Samples",
                column: "ChartTemplateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biases");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "ChartTemplates");

            migrationBuilder.DropTable(
                name: "MeasurementTypes");
        }
    }
}
