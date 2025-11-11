using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Unitforchart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ChartTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Represents unit of Y axis");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Device_Spread_Range",
                table: "Devices",
                sql: "Spread >= 0 AND Spread <= 100");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Device_Spread_Range",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ChartTemplates");
        }
    }
}
