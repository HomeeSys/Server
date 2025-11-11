using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Locationcanhavemultiplecharts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biases");

            migrationBuilder.AddColumn<double>(
                name: "Spread",
                table: "Devices",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Measurement value spread, expressed in percentage.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartTemplates",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Name of chart, for example 'Temperature' it should represent measurement type.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Name of chart, for example 'Temperature' it should represent measurement type.");

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "ChartTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeOffset",
                table: "ChartTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Move chart to the right or left side.");

            migrationBuilder.AddColumn<double>(
                name: "ValueOffset",
                table: "ChartTemplates",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Boost all values of this chart.");

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_LocationID",
                table: "ChartTemplates",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_Name",
                table: "ChartTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartTemplates_Locations_LocationID",
                table: "ChartTemplates",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartTemplates_Locations_LocationID",
                table: "ChartTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChartTemplates_LocationID",
                table: "ChartTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChartTemplates_Name",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "Spread",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "ValueOffset",
                table: "ChartTemplates");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartTemplates",
                type: "nvarchar(max)",
                nullable: false,
                comment: "Name of chart, for example 'Temperature' it should represent measurement type.",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldComment: "Name of chart, for example 'Temperature' it should represent measurement type.");

            migrationBuilder.CreateTable(
                name: "Biases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false),
                    DeviceID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    TimeOffset = table.Column<int>(type: "int", nullable: false),
                    ValueOffset = table.Column<double>(type: "float", nullable: false),
                    ValueSpread = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biases", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Biases_ChartTemplates_ChartTemplateID",
                        column: x => x.ChartTemplateID,
                        principalTable: "ChartTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biases_ChartTemplateID",
                table: "Biases",
                column: "ChartTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Biases_DeviceID",
                table: "Biases",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_Biases_LocationID",
                table: "Biases",
                column: "LocationID");
        }
    }
}
