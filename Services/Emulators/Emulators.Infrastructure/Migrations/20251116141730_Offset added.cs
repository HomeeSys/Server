using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Offsetadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartTemplates_Locations_LocationID",
                table: "ChartTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChartTemplates_LocationID",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "ValueOffset",
                table: "ChartTemplates");

            migrationBuilder.CreateTable(
                name: "Offsets",
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
                    table.PrimaryKey("PK_Offsets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Offsets_ChartTemplates_ChartTemplateID",
                        column: x => x.ChartTemplateID,
                        principalTable: "ChartTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offsets_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offsets_ChartTemplateID",
                table: "Offsets",
                column: "ChartTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_Offsets_LocationID",
                table: "Offsets",
                column: "LocationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offsets");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ChartTemplates_Locations_LocationID",
                table: "ChartTemplates",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
