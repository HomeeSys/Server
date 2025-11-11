using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removeduniquechartname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChartTemplates_Name",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartTemplates",
                type: "nvarchar(450)",
                nullable: false,
                comment: "Name of chart, for example 'Temperature' it should represent measurement type.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldComment: "Name of chart, for example 'Temperature' it should represent measurement type.");

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_Name",
                table: "ChartTemplates",
                column: "Name",
                unique: true);
        }
    }
}
