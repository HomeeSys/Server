using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Movedmeasurementtypeoutsidechart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ChartTemplates");

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of measurement, for example 'Air Temperature'."),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Unit"),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Measurement_ChartTemplates_ChartTemplateID",
                        column: x => x.ChartTemplateID,
                        principalTable: "ChartTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurement_ChartTemplateID",
                table: "Measurement",
                column: "ChartTemplateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ChartTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Name of chart, for example 'Temperature' it should represent measurement type.");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ChartTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Represents unit of Y axis");
        }
    }
}
