using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteerManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_IncidentReports_IncidentId",
                table: "Volunteers");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Tasks_TaskId",
                table: "Volunteers");

            migrationBuilder.DropTable(
                name: "Tasks");

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
        }
    }
}
