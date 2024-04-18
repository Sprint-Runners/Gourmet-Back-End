using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.Domain.Relations;

namespace Gourmet.Core.Services
{
    public class RecipeService:IRecipeService
    {

        private readonly AppDbContext _db;
        private readonly UserManager<Chef> _userManager;
        private readonly IImageProcessorService _imageProcessorService;
        public RecipeService(AppDbContext db, UserManager<Chef> userManager, IImageProcessorService imageProcessorService)
        {
            _db = db;
            _userManager = userManager;
            _imageProcessorService = imageProcessorService;
        }
        public async Task<RecipeResponse> CreateRecipeByChef(AddRecipeRequest request, string userId, string username)
        {
            var isExitsFood = _db.Foods.Where(x => x.Name == request.FoodName).FirstOrDefault();
            var isExistsRecipe = _db.Recipes.Where(x => x.FoodId == isExitsFood.Id).Where(x => x.ChefId == userId).FirstOrDefault();
            var isExitsUser = await _userManager.FindByNameAsync(username);
            var isExitsPSOI = _db.PSOIs.Where(x => x.Name == request.primary_source_of_ingredient).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Name == request.cooking_method).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Name == request.food_type).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Name == request.nationality).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Name == request.meal_type).FirstOrDefault();
            if (isExistsRecipe != null)
                return new RecipeResponse()
                {
                    IsSucceed = false,
                    Message = "Chefs Recipe Already Exists",
                    recipe = null
                };
            if (isExitsUser == null)
                return new RecipeResponse()
                {
                    IsSucceed = false,
                    Message = "Chefs not found",
                    recipe = null
                };
            if (isExitsFood == null)
                return new RecipeResponse()
                {
                    IsSucceed = false,
                    Message = "The Food not found",
                    recipe = null
                };
            string ingredients = "";
            foreach(var item in request.List_Ingriedents)
            {
                ingredients = ingredients +","+ item.Item1;
            }
            Recipe new_recipe = new Recipe()
            {
                Id = new Guid(),
                FoodId = isExitsFood.Id,
                food = isExitsFood,
                ChefId = userId,
                chef = isExitsUser,
                Description = request.Description,
                Score = 0,
                ImgeUrl =await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName),
                List_Ingriedents = ingredients,
                Primary_Source_of_IngredientId = isExitsPSOI.Id,
                primary_source_of_ingredient = isExitsPSOI,
                Cooking_MethodId = isExitsCM.Id,
                cooking_method = isExitsCM,
                Food_typeId = isExitsFT.Id,
                food_type = isExitsFT,
                NationalityId = isExitsN.Id,
                nationality = isExitsN,
                Meal_TypeId = isExitsMT.Id,
                meal_type = isExitsMT
            };
            _db.Recipes.Add(new_recipe);
            foreach(var item in request.List_Ingriedents)
            {
                var isExitsIngredient = _db.Ingredients.Where(x => x.Name == item.Item1).FirstOrDefault();
                RecipeIngredients row = new RecipeIngredients
                {
                    RecipeId =new_recipe.Id,
                    recipe =new_recipe,
                    IngredientId =isExitsIngredient.Id,
                    ingredient =isExitsIngredient,
                    Quantity =item.Item2
                };
                _db.RecipeIngredients.Add(row);
            }
            _db.SaveChanges();

            return new RecipeResponse()
            {
                IsSucceed = true,
                Message = "Recipe Added Successfully",
                recipe = new_recipe
            };
            
        }
        public async Task<InCompleteRecipeResponse> CreateInCompleteRecipe(AddRecipeRequest request, string userId, string username)
        {
            var isExitsUser = await _userManager.FindByNameAsync(username);
            var isExitsPSOI = _db.PSOIs.Where(x => x.Name == request.primary_source_of_ingredient).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Name == request.cooking_method).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Name == request.food_type).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Name == request.nationality).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Name == request.meal_type).FirstOrDefault();
            if (isExitsUser == null)
                return new InCompleteRecipeResponse()
                {
                    IsSucceed = false,
                    Message = "Chefs not found",
                    recipe = null
                };
            string ingredients = "";
            string NotExistingredients = "";
            foreach (var item in request.List_Ingriedents)
            {
                string ingredient = item.Item1 + "," + item.Item2;
                ingredients = ingredients+ingredient+".";
            }
            foreach (var item in request.Not_Exist_List_Ingriedents)
            {
                string ingredient = item.Item1 + "," + item.Item2+","+item.Item3;
                NotExistingredients = NotExistingredients + ingredient + ".";
            }
            InCompleteRecipe new_recipe = new InCompleteRecipe()
            {
                Id = new Guid(),
                FoodString=request.FoodName,
                ChefId = userId,
                chef = isExitsUser,
                Description = request.Description,
                ImgeUrl =await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName),
                IngredientsString=ingredients,
                NotExistIngredients=NotExistingredients,
                Primary_Source_of_IngredientId = isExitsPSOI.Id,
                primary_source_of_ingredient = isExitsPSOI,
                Cooking_MethodId = isExitsCM.Id,
                cooking_method = isExitsCM,
                Food_typeId = isExitsFT.Id,
                food_type = isExitsFT,
                NationalityId = isExitsN.Id,
                nationality = isExitsN,
                Meal_TypeId = isExitsMT.Id,
                meal_type = isExitsMT
            };
            _db.InCompleteRecipes.Add(new_recipe);
            _db.SaveChanges();

            return new InCompleteRecipeResponse()
            {
                IsSucceed = true,
                Message = "Recipe Added Successfully",
                recipe = new_recipe
            };

        }

    }
}
