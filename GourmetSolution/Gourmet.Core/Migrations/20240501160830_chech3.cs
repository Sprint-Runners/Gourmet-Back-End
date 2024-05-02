using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class chech3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgeUrl",
                table: "Recipes",
                newName: "ImgeUrl5");

            migrationBuilder.RenameColumn(
                name: "ImgeUrl",
                table: "InCompleteRecipes",
                newName: "ImgeUrl5");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl1",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl2",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl3",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl4",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPicture",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl1",
                table: "InCompleteRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl2",
                table: "InCompleteRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl3",
                table: "InCompleteRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgeUrl4",
                table: "InCompleteRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPicture",
                table: "InCompleteRecipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgeUrl1",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl2",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl3",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl4",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "NumberOfPicture",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl1",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl2",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl3",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "ImgeUrl4",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "NumberOfPicture",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImgeUrl5",
                table: "Recipes",
                newName: "ImgeUrl");

            migrationBuilder.RenameColumn(
                name: "ImgeUrl5",
                table: "InCompleteRecipes",
                newName: "ImgeUrl");
        }
    }
}
