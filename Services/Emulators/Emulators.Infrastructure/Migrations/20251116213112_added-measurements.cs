using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedmeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Name of measurement, for example 'Air Temperature'."),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Unit"),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Measurements_ChartTemplates_ChartTemplateID",
                        column: x => x.ChartTemplateID,
                        principalTable: "ChartTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ChartTemplateID",
                table: "Measurements",
                column: "ChartTemplateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");
        }
    }
}
