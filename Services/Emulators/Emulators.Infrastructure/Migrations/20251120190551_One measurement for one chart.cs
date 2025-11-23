using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Onemeasurementforonechart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_ChartTemplates_ChartTemplateID",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_ChartTemplateID",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "ChartTemplateID",
                table: "Measurements");

            migrationBuilder.AddColumn<int>(
                name: "MeasurementID",
                table: "ChartTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_MeasurementID",
                table: "ChartTemplates",
                column: "MeasurementID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartTemplates_Measurements_MeasurementID",
                table: "ChartTemplates",
                column: "MeasurementID",
                principalTable: "Measurements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartTemplates_Measurements_MeasurementID",
                table: "ChartTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ChartTemplates_MeasurementID",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "MeasurementID",
                table: "ChartTemplates");

            migrationBuilder.AddColumn<int>(
                name: "ChartTemplateID",
                table: "Measurements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ChartTemplateID",
                table: "Measurements",
                column: "ChartTemplateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_ChartTemplates_ChartTemplateID",
                table: "Measurements",
                column: "ChartTemplateID",
                principalTable: "ChartTemplates",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
