using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raports.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _3HashAddedToLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "Locations");
        }
    }
}
