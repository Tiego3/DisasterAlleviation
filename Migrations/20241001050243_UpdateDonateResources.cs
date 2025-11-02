using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDonateResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Donations",
                newName: "NumberOfItems");

            migrationBuilder.RenameColumn(
                name: "DonatedAt",
                table: "Donations",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "NumberOfItems",
                table: "Donations",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Donations",
                newName: "DonatedAt");
        }
    }
}
