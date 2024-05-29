using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class check5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InCompleteRecipes");

            migrationBuilder.AddColumn<string>(
                name: "FoodString",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompelete",
                table: "Recipes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NotExistIngredients",
                table: "Recipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "premium",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodString",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IsCompelete",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "NotExistIngredients",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "premium",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "InCompleteRecipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Cooking_MethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Difficulty_LevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Food_typeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Meal_Type = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Primary_Source_of_IngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: false),
                    FoodString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgeUrl1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgeUrl2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgeUrl3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgeUrl4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgeUrl5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IngredientsString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Meal_TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotExistIngredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfPicture = table.Column<int>(type: "int", nullable: false),
                    StepsString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InCompleteRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_AspNetUsers_ChefId",
                        column: x => x.ChefId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_CMs_Cooking_MethodId",
                        column: x => x.Cooking_MethodId,
                        principalTable: "CMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_DLs_Difficulty_LevelId",
                        column: x => x.Difficulty_LevelId,
                        principalTable: "DLs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_FTs_Food_typeId",
                        column: x => x.Food_typeId,
                        principalTable: "FTs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_MTs_Meal_Type",
                        column: x => x.Meal_Type,
                        principalTable: "MTs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_Ns_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Ns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InCompleteRecipes_PSOIs_Primary_Source_of_IngredientId",
                        column: x => x.Primary_Source_of_IngredientId,
                        principalTable: "PSOIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_ChefId",
                table: "InCompleteRecipes",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_Cooking_MethodId",
                table: "InCompleteRecipes",
                column: "Cooking_MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_Difficulty_LevelId",
                table: "InCompleteRecipes",
                column: "Difficulty_LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_Food_typeId",
                table: "InCompleteRecipes",
                column: "Food_typeId");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_Meal_Type",
                table: "InCompleteRecipes",
                column: "Meal_Type");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_NationalityId",
                table: "InCompleteRecipes",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_InCompleteRecipes_Primary_Source_of_IngredientId",
                table: "InCompleteRecipes",
                column: "Primary_Source_of_IngredientId");
        }
    }
}
