using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gourmet.Core.Migrations
{
    public partial class check2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "aboutme",
                table: "AspNetUsers",
                newName: "Aboutme");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "RecipeIngredients",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Ingredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToLike",
                table: "FavouritFoodUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "InCompleteRecipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FoodString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChefId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 15000, nullable: false),
                    ImgeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IngredientsString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotExistIngredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Primary_Source_of_IngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cooking_MethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Food_typeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Meal_TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Meal_Type = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InCompleteRecipes");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "TimeToLike",
                table: "FavouritFoodUsers");

            migrationBuilder.RenameColumn(
                name: "Aboutme",
                table: "AspNetUsers",
                newName: "aboutme");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
