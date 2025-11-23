using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Movedmeasurementtypeoutsidechart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurement_ChartTemplates_ChartTemplateID",
                table: "Measurement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Measurement",
                table: "Measurement");

            migrationBuilder.RenameTable(
                name: "Measurement",
                newName: "Measurements");

            migrationBuilder.RenameIndex(
                name: "IX_Measurement_ChartTemplateID",
                table: "Measurements",
                newName: "IX_Measurements_ChartTemplateID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measurements",
                table: "Measurements",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_ChartTemplates_ChartTemplateID",
                table: "Measurements",
                column: "ChartTemplateID",
                principalTable: "ChartTemplates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_ChartTemplates_ChartTemplateID",
                table: "Measurements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Measurements",
                table: "Measurements");

            migrationBuilder.RenameTable(
                name: "Measurements",
                newName: "Measurement");

            migrationBuilder.RenameIndex(
                name: "IX_Measurements_ChartTemplateID",
                table: "Measurement",
                newName: "IX_Measurement_ChartTemplateID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measurement",
                table: "Measurement",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurement_ChartTemplates_ChartTemplateID",
                table: "Measurement",
                column: "ChartTemplateID",
                principalTable: "ChartTemplates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
