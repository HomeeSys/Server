using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emulators.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeletedMeasurementType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biases_MeasurementTypes_MeasurementTypeID",
                table: "Biases");

            migrationBuilder.DropForeignKey(
                name: "FK_ChartTemplates_MeasurementTypes_MeasurementTypeID",
                table: "ChartTemplates");

            migrationBuilder.DropTable(
                name: "MeasurementTypes");

            migrationBuilder.DropIndex(
                name: "IX_ChartTemplates_MeasurementTypeID",
                table: "ChartTemplates");

            migrationBuilder.DropColumn(
                name: "MeasurementTypeID",
                table: "ChartTemplates");

            migrationBuilder.RenameColumn(
                name: "MeasurementTypeID",
                table: "Biases",
                newName: "ChartTemplateID");

            migrationBuilder.RenameIndex(
                name: "IX_Biases_MeasurementTypeID",
                table: "Biases",
                newName: "IX_Biases_ChartTemplateID");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ChartTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Name of chart, for example 'Temperature' it should represent measurement type.");

            migrationBuilder.AddForeignKey(
                name: "FK_Biases_ChartTemplates_ChartTemplateID",
                table: "Biases",
                column: "ChartTemplateID",
                principalTable: "ChartTemplates",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biases_ChartTemplates_ChartTemplateID",
                table: "Biases");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ChartTemplates");

            migrationBuilder.RenameColumn(
                name: "ChartTemplateID",
                table: "Biases",
                newName: "MeasurementTypeID");

            migrationBuilder.RenameIndex(
                name: "IX_Biases_ChartTemplateID",
                table: "Biases",
                newName: "IX_Biases_MeasurementTypeID");

            migrationBuilder.AddColumn<int>(
                name: "MeasurementTypeID",
                table: "ChartTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MeasurementTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartTemplateID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementTypes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartTemplates_MeasurementTypeID",
                table: "ChartTemplates",
                column: "MeasurementTypeID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Biases_MeasurementTypes_MeasurementTypeID",
                table: "Biases",
                column: "MeasurementTypeID",
                principalTable: "MeasurementTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartTemplates_MeasurementTypes_MeasurementTypeID",
                table: "ChartTemplates",
                column: "MeasurementTypeID",
                principalTable: "MeasurementTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
