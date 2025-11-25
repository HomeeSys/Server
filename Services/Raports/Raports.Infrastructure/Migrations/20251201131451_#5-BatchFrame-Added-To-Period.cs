using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raports.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _5BatchFrameAddedToPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxAcceptableMissingTimeFrame",
                table: "Periods",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeFrame",
                table: "Periods",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddCheckConstraint(
                name: "CK_Period_MaxAcceptableMissingTimeFrame_Range",
                table: "Periods",
                sql: "MaxAcceptableMissingTimeFrame >= 1 AND MaxAcceptableMissingTimeFrame <= 100");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Period_MaxAcceptableMissingTimeFrame_Range",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "MaxAcceptableMissingTimeFrame",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "Periods");
        }
    }
}
