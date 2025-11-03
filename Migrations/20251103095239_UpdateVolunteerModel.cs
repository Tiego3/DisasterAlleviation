using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVolunteerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_IncidentReports_IncidentId",
                table: "Volunteers");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Tasks_TaskId",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Volunteers_IncidentId",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Volunteers_TaskId",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Volunteers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Volunteers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "ContactInfo",
                table: "Volunteers",
                newName: "Location");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Availability",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableDates",
                table: "Volunteers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasTransportation",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PreviousExperience",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WillingToTravel",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "AvailableDates",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "HasTransportation",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "PreviousExperience",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "WillingToTravel",
                table: "Volunteers");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Volunteers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Volunteers",
                newName: "ContactInfo");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Availability",
                table: "Volunteers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "Volunteers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Volunteers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_IncidentId",
                table: "Volunteers",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_TaskId",
                table: "Volunteers",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_IncidentReports_IncidentId",
                table: "Volunteers",
                column: "IncidentId",
                principalTable: "IncidentReports",
                principalColumn: "IncidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Tasks_TaskId",
                table: "Volunteers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
