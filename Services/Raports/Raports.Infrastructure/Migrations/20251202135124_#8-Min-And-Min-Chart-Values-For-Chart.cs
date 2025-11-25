using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raports.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _8MinAndMinChartValuesForChart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxChartYValue",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinChartYValue",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxChartYValue",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "MinChartYValue",
                table: "Measurements");
        }
    }
}
