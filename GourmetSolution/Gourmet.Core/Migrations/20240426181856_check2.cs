using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class check2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_DFs_Difficulty_LevelId",
                table: "Recipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DFs",
                table: "DFs");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredients");

            migrationBuilder.RenameTable(
                name: "DFs",
                newName: "DLs");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatTime",
                table: "Recipes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RecipeIngredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Difficulty_LevelId",
                table: "InCompleteRecipes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "InCompleteRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StepsString",
                table: "InCompleteRecipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "InCompleteRecipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DLs",
                table: "DLs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_Difficulty_LevelId",
                table: "InCompleteRecipes",
                column: "Difficulty_LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InCompleteRecipes_DLs_Difficulty_LevelId",
                table: "InCompleteRecipes",
                column: "Difficulty_LevelId",
                principalTable: "DLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_DLs_Difficulty_LevelId",
                table: "Recipes",
                column: "Difficulty_LevelId",
                principalTable: "DLs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InCompleteRecipes_DLs_Difficulty_LevelId",
                table: "InCompleteRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_DLs_Difficulty_LevelId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_InCompleteRecipes_Difficulty_LevelId",
                table: "InCompleteRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DLs",
                table: "DLs");

            migrationBuilder.DropColumn(
                name: "CreatTime",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Difficulty_LevelId",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "StepsString",
                table: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "InCompleteRecipes");

            migrationBuilder.RenameTable(
                name: "DLs",
                newName: "DFs");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DFs",
                table: "DFs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_DFs_Difficulty_LevelId",
                table: "Recipes",
                column: "Difficulty_LevelId",
                principalTable: "DFs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
