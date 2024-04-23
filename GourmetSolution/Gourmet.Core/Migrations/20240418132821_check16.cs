using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class check16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PSOIs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Ns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MTs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "FTs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "CMs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PSOIs");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Ns");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MTs");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "FTs");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "CMs");
        }
    }
}
