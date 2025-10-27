using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raports.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "For example: 'Daily', 'Weekly', 'Monthly', etc...")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "For example: 'Generated', 'Pending', etc..."),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "What is exactly happening here")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when request was created"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Raport will be generate from this date"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Raport will be generate to this date"),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    RaportID = table.Column<int>(type: "int", nullable: false),
                    PeriodID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Requests_Periods_PeriodID",
                        column: x => x.PeriodID,
                        principalTable: "Periods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_RequestStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "RequestStatuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Raports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when raport was created"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodID = table.Column<int>(type: "int", nullable: false),
                    RequestID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Raports_Periods_PeriodID",
                        column: x => x.PeriodID,
                        principalTable: "Periods",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Raports_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Raports_PeriodID",
                table: "Raports",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_Raports_RequestID",
                table: "Raports",
                column: "RequestID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PeriodID",
                table: "Requests",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StatusID",
                table: "Requests",
                column: "StatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Raports");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "RequestStatuses");
        }
    }
}
