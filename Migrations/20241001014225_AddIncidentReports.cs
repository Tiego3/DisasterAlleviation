using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class AddIncidentReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IncidentReports");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IncidentReports",
                newName: "IncidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IncidentId",
                table: "IncidentReports",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "IncidentReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
