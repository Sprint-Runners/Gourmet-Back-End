using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class check4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_MTs_Meal_Type",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "FavouritFoodUsers");

            migrationBuilder.DropTable(
                name: "RecentFoodUsers");

            migrationBuilder.RenameColumn(
                name: "Meal_Type",
                table: "Recipes",
                newName: "Difficulty_LevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_Meal_Type",
                table: "Recipes",
                newName: "IX_Recipes_Difficulty_LevelId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 15000);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Recipes",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DFs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DFs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FavouritRecipeUsers",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeToLike = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouritRecipeUsers", x => new { x.userId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_FavouritRecipeUsers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouritRecipeUsers_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecentRecipeUsers",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisitTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentRecipeUsers", x => new { x.userId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_RecentRecipeUsers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentRecipeUsers_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeSteps",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    explenation = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSteps", x => new { x.RecipeId, x.Number });
                    table.ForeignKey(
                        name: "FK_RecipeSteps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Meal_TypeId",
                table: "Recipes",
                column: "Meal_TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouritRecipeUsers_RecipeId",
                table: "FavouritRecipeUsers",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentRecipeUsers_RecipeId",
                table: "RecentRecipeUsers",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_DFs_Difficulty_LevelId",
                table: "Recipes",
                column: "Difficulty_LevelId",
                principalTable: "DFs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_MTs_Meal_TypeId",
                table: "Recipes",
                column: "Meal_TypeId",
                principalTable: "MTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_DFs_Difficulty_LevelId",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_MTs_Meal_TypeId",
                table: "Recipes");

            migrationBuilder.DropTable(
                name: "DFs");

            migrationBuilder.DropTable(
                name: "FavouritRecipeUsers");

            migrationBuilder.DropTable(
                name: "RecentRecipeUsers");

            migrationBuilder.DropTable(
                name: "RecipeSteps");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_Meal_TypeId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Recipes");

            migrationBuilder.RenameColumn(
                name: "Difficulty_LevelId",
                table: "Recipes",
                newName: "Meal_Type");

            migrationBuilder.RenameIndex(
                name: "IX_Recipes_Difficulty_LevelId",
                table: "Recipes",
                newName: "IX_Recipes_Meal_Type");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Recipes",
                type: "nvarchar(max)",
                maxLength: 15000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateTable(
                name: "FavouritFoodUsers",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeToLike = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouritFoodUsers", x => new { x.userId, x.FoodId });
                    table.ForeignKey(
                        name: "FK_FavouritFoodUsers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouritFoodUsers_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecentFoodUsers",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FoodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentFoodUsers", x => new { x.userId, x.FoodId });
                    table.ForeignKey(
                        name: "FK_RecentFoodUsers_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentFoodUsers_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouritFoodUsers_FoodId",
                table: "FavouritFoodUsers",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentFoodUsers_FoodId",
                table: "RecentFoodUsers",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_MTs_Meal_Type",
                table: "Recipes",
                column: "Meal_Type",
                principalTable: "MTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
