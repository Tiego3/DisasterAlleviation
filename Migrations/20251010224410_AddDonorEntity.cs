using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class AddDonorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DonorId",
                table: "MonetaryDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DonorId",
                table: "GoodsDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonetaryDonations_DonorId",
                table: "MonetaryDonations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsDonations_DonorId",
                table: "GoodsDonations",
                column: "DonorId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoodsDonations_Donors_DonorId",
                table: "GoodsDonations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonetaryDonations_Donors_DonorId",
                table: "MonetaryDonations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoodsDonations_Donors_DonorId",
                table: "GoodsDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_MonetaryDonations_Donors_DonorId",
                table: "MonetaryDonations");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropIndex(
                name: "IX_MonetaryDonations_DonorId",
                table: "MonetaryDonations");

            migrationBuilder.DropIndex(
                name: "IX_GoodsDonations_DonorId",
                table: "GoodsDonations");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "MonetaryDonations");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "GoodsDonations");
        }
    }
}
