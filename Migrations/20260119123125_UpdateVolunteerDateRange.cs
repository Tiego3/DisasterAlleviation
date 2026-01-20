using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVolunteerDateRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvailableDates",
                table: "Volunteers",
                newName: "AvailableUntilDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFromDate",
                table: "Volunteers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableFromDate",
                table: "Volunteers");

            migrationBuilder.RenameColumn(
                name: "AvailableUntilDate",
                table: "Volunteers",
                newName: "AvailableDates");
        }
    }
}
