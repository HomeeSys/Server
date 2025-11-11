using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renamedoffets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offsets");

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

            migrationBuilder.CreateIndex(
                name: "IX_ChartOffsets_ChartTemplateID",
                table: "ChartOffsets",
                column: "ChartTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOffsets_LocationID",
                table: "ChartOffsets",
                column: "LocationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChartOffsets");

            migrationBuilder.CreateTable(
                name: "Offsets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false)
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
    }
}
