using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raports.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "For example: 'Kitchen', 'Attic' etc..."),
                    Hash = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinChartYValue = table.Column<int>(type: "int", nullable: false),
                    MaxChartYValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "For example: 'Daily', 'Weekly', 'Monthly', etc..."),
                    TimeFrame = table.Column<TimeSpan>(type: "time", nullable: false, defaultValue: new TimeSpan(0, 0, 0, 0, 0)),
                    MaxAcceptableMissingTimeFrame = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.ID);
                    table.CheckConstraint("CK_Period_MaxAcceptableMissingTimeFrame_Range", "MaxAcceptableMissingTimeFrame >= 1 AND MaxAcceptableMissingTimeFrame <= 100");
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Raports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaportCreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of Raport creation"),
                    RaportCompletedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of Raport completion"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of first measurement"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of last measurement"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentHash = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"), comment: "Hash that allows to identify PDF in Azure Blob Storage"),
                    PeriodID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_Raports_Statuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "Statuses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementGroups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Combined summary for all location groups"),
                    RaportID = table.Column<int>(type: "int", nullable: false),
                    MeasurementID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MeasurementGroups_Measurements_MeasurementID",
                        column: x => x.MeasurementID,
                        principalTable: "Measurements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeasurementGroups_Raports_RaportID",
                        column: x => x.RaportID,
                        principalTable: "Raports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestedLocations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    RaportID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedLocations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestedLocations_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedLocations_Raports_RaportID",
                        column: x => x.RaportID,
                        principalTable: "Raports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestedMeasurements",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementID = table.Column<int>(type: "int", nullable: false),
                    RaportID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedMeasurements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestedMeasurements_Measurements_MeasurementID",
                        column: x => x.MeasurementID,
                        principalTable: "Measurements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedMeasurements_Raports_RaportID",
                        column: x => x.RaportID,
                        principalTable: "Raports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationGroups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Verbal summary of data stored for this location"),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    MeasurementGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LocationGroups_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationGroups_MeasurementGroups_MeasurementGroupID",
                        column: x => x.MeasurementGroupID,
                        principalTable: "MeasurementGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SampleGroups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Time of measurement"),
                    Value = table.Column<double>(type: "float", nullable: false, comment: "Value  of measurement"),
                    LocationGroupID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SampleGroups_LocationGroups_LocationGroupID",
                        column: x => x.LocationGroupID,
                        principalTable: "LocationGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationGroups_LocationID",
                table: "LocationGroups",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationGroups_MeasurementGroupID",
                table: "LocationGroups",
                column: "MeasurementGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementGroups_MeasurementID",
                table: "MeasurementGroups",
                column: "MeasurementID");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementGroups_RaportID",
                table: "MeasurementGroups",
                column: "RaportID");

            migrationBuilder.CreateIndex(
                name: "IX_Raports_PeriodID",
                table: "Raports",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_Raports_StatusID",
                table: "Raports",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedLocations_LocationID",
                table: "RequestedLocations",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedLocations_RaportID",
                table: "RequestedLocations",
                column: "RaportID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedMeasurements_MeasurementID",
                table: "RequestedMeasurements",
                column: "MeasurementID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedMeasurements_RaportID",
                table: "RequestedMeasurements",
                column: "RaportID");

            migrationBuilder.CreateIndex(
                name: "IX_SampleGroups_LocationGroupID",
                table: "SampleGroups",
                column: "LocationGroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestedLocations");

            migrationBuilder.DropTable(
                name: "RequestedMeasurements");

            migrationBuilder.DropTable(
                name: "SampleGroups");

            migrationBuilder.DropTable(
                name: "LocationGroups");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MeasurementGroups");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "Raports");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
