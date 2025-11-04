using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class AddDisasterRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisasterId",
                table: "Volunteers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_DisasterId",
                table: "Volunteers",
                column: "DisasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Disaster_DisasterId",
                table: "Volunteers",
                column: "DisasterId",
                principalTable: "Disaster",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Disaster_DisasterId",
                table: "Volunteers");

            migrationBuilder.DropIndex(
                name: "IX_Volunteers_DisasterId",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "DisasterId",
                table: "Volunteers");
        }
    }
}
