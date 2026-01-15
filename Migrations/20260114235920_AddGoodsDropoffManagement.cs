using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterAlleviation.Migrations
{
    /// <inheritdoc />
    public partial class AddGoodsDropoffManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "GoodsDonations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "GoodsDonations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DropoffDateTime",
                table: "GoodsDonations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropoffStatus",
                table: "GoodsDonations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "GoodsDonations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "GoodsDonations");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "GoodsDonations");

            migrationBuilder.DropColumn(
                name: "DropoffDateTime",
                table: "GoodsDonations");

            migrationBuilder.DropColumn(
                name: "DropoffStatus",
                table: "GoodsDonations");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "GoodsDonations");
        }
    }
}
