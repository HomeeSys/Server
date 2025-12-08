using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1Initial : Migration
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
                    DeviceNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "This is unique name for device"),
                    Spread = table.Column<double>(type: "float", nullable: false, comment: "Measurement value spread, expressed in percentage.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.ID);
                    table.CheckConstraint("CK_Device_Spread_Range", "Spread >= 0 AND Spread <= 100");
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hash = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of measurement, for example 'Air Temperature'."),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Unit")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChartTemplates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartTemplates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChartTemplates_Measurements_MeasurementID",
                        column: x => x.MeasurementID,
                        principalTable: "Measurements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartOffsets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartOffsets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChartOffsets_ChartTemplates_ChartTemplateID",
                        column: x => x.ChartTemplateID,
                        principalTable: "ChartTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartOffsets_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
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
                name: "IX_ChartOffsets_ChartTemplateID",
                table: "ChartOffsets",
                column: "ChartTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOffsets_LocationID",
                table: "ChartOffsets",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_MeasurementID",
                table: "ChartTemplates",
                column: "MeasurementID");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceNumber",
                table: "Devices",
                column: "DeviceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name",
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
                name: "ChartOffsets");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "ChartTemplates");

            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
