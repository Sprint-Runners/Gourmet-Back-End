using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.Domain.Relations;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace Gourmet.Core.Services
{
    public class RecipeService : IRecipeService
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
            var allPSOI = _db.PSOIs.ToList();
            var isExitsFood = _db.Foods.Where(x => x.Name.ToLower() == request.FoodName.ToLower()).FirstOrDefault();
            var isExistsRecipe = _db.Recipes.Where(x => x.FoodId == isExitsFood.Id).Where(x => x.ChefId == userId).Where(x => x.Name == request.Name).FirstOrDefault();
            var isExitsUser = await _userManager.FindByNameAsync(username);
            var isExitsPSOI = allPSOI.Where(x => x.Name == request.primary_source_of_ingredient).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Name == request.cooking_method).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Name == request.food_type).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Name == request.nationality).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Name == request.meal_type).FirstOrDefault();
            var isExistDL = _db.DLs.Where(x => x.Name == request.difficulty_level).FirstOrDefault();
            Console.WriteLine("kdfuihfiuhfiuhuih*******************");
            if (isExistsRecipe != null)
                return new RecipeResponse()
                {
                    IsSucceed = false,
                    Message = "Chefs Recipe Already Exists",
                    recipe = null
                };
            //isExistsRecipe = _db.Recipes.Where(x => x.FoodString == isExitsFood.Name).Where(x => x.ChefId == userId).Where(x => x.Name == request.Name).FirstOrDefault();

            //if (isExistsRecipe != null)
            //    return new RecipeResponse()
            //    {
            //        IsSucceed = false,
            //        Message = "Chefs Recipe Already Exists",
            //        recipe = null
            //    };
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
            foreach (var item in request.List_Ingriedents)
            {
                string ingredient = item.Item1 + "," + item.Item2 + "," + item.Item3;
                ingredients = ingredients + ingredient + ".";
            }
            Recipe new_recipe = new Recipe()
            {
                Id = new Guid(),
                FoodId = isExitsFood.Id,
                Name = request.Name,
                ChefId = userId,
                Description = request.Description,
                Score = 0,
                ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 1),
                ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 2),
                ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 3),
                ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 4),
                ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 5),
                List_Ingriedents = ingredients,
                Primary_Source_of_IngredientId = isExitsPSOI.Id,
                Cooking_MethodId = isExitsCM.Id,
                Food_typeId = isExitsFT.Id,
                NationalityId = isExitsN.Id,
                Meal_TypeId = isExitsMT.Id,
                Difficulty_LevelId = isExistDL.Id,
                Time = int.Parse(request.Time),
                IsAccepted = false,
                IsReject = false,
                FoodString = "",
                NotExistIngredients = "",
                IsCompelete = true

            };
            _db.Recipes.Add(new_recipe);
            foreach (var item in request.List_Ingriedents)
            {
                var isExitsIngredient = _db.Ingredients.Where(x => x.Name == item.Item1).FirstOrDefault();
                RecipeIngredients row = new RecipeIngredients
                {
                    RecipeId = new_recipe.Id,
                    IngredientId = isExitsIngredient.Id,
                    Quantity = double.Parse(item.Item2),
                    Unit = item.Item3
                };
                _db.RecipeIngredients.Add(row);
            }
            foreach (var item in request.Steps)
            {
                RecipeStep row = new RecipeStep
                {
                    RecipeId = new_recipe.Id,
                    explenation = item.Item2,
                    Number = int.Parse(item.Item1)
                };
                _db.RecipeSteps.Add(row);
            }
            _db.SaveChanges();

            return new RecipeResponse()
            {
                IsSucceed = true,
                Message = "Recipe Added Successfully",
                recipe = new_recipe
            };

        }
        public async Task<RecipeResponse> CreateInCompleteRecipe(AddRecipeRequest request, string userId, string username)
        {
            var isExitsFood = _db.Foods.Where(x => x.Name.ToLower() == request.FoodName.ToLower()).FirstOrDefault();
            if (isExitsFood == null)
            {
                var isExistsRecipe = _db.Recipes.Where(x => x.FoodString == request.FoodName).Where(x => x.ChefId == userId).Where(x => x.Name == request.Name).FirstOrDefault();
                if (isExistsRecipe != null)
                    return new RecipeResponse()
                    {
                        IsSucceed = false,
                        Message = "Chefs Recipe Already Exists",
                        recipe = null
                    };
            }
            else
            {
                var isExistsRecipe = _db.Recipes.Where(x => x.FoodId == isExitsFood.Id).Where(x => x.ChefId == userId).Where(x => x.Name == request.Name).FirstOrDefault();
                if (isExistsRecipe != null)
                    return new RecipeResponse()
                    {
                        IsSucceed = false,
                        Message = "Chefs Recipe Already Exists",
                        recipe = null
                    };
            }
            var isExitsUser = await _userManager.FindByNameAsync(username);
            var isExitsPSOI = _db.PSOIs.Where(x => x.Name == request.primary_source_of_ingredient).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Name == request.cooking_method).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Name == request.food_type).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Name == request.nationality).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Name == request.meal_type).FirstOrDefault();
            var isExistDL = _db.DLs.Where(x => x.Name == request.difficulty_level).FirstOrDefault();
            if (isExitsUser == null)
                return new RecipeResponse()
                {
                    IsSucceed = false,
                    Message = "Chefs not found",
                    recipe = null
                };
            string ingredients = "";
            string NotExistingredients = "";
            string Steps = "";
            var ExistIngredients = request.List_Ingriedents.Where(ing => ing.Item4 == true).ToList();
            foreach (var item in ExistIngredients)
            {
                string ingredient = item.Item1 + "," + item.Item2 + "," + item.Item3;
                ingredients = ingredients + ingredient + ".";
            }
            var NotExistIngredients = request.List_Ingriedents.Where(ing => ing.Item4 == false).ToList();
            foreach (var item in NotExistIngredients)
            {
                string ingredient = item.Item1 + "," + item.Item2 + "," + item.Item3;
                NotExistingredients = NotExistingredients + ingredient + ".";
            }

            Recipe new_recipe = new Recipe()
            {
                Id = new Guid(),
                Name = request.Name,
                ChefId = userId,
                Description = request.Description,
                Score = 0,
                List_Ingriedents = ingredients,
                NotExistIngredients = NotExistingredients,
                Primary_Source_of_IngredientId = isExitsPSOI.Id,
                Cooking_MethodId = isExitsCM.Id,
                Food_typeId = isExitsFT.Id,
                NationalityId = isExitsN.Id,
                Meal_TypeId = isExitsMT.Id,
                Difficulty_LevelId = isExistDL.Id,
                Time = int.Parse(request.Time),
                IsAccepted = false,
                IsReject = false,
                IsCompelete=false

            };
            if (isExitsFood != null)
            {
                new_recipe.FoodId = isExitsFood.Id;
                new_recipe.FoodString = "";
                new_recipe.ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 1);
                new_recipe.ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 2);
                new_recipe.ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 3);
                new_recipe.ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 4);
                new_recipe.ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitsUser.UserName, request.Name, 5);
            }
            else
            {
                new_recipe.FoodString = request.NotExistFoodName;
                new_recipe.ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(new_recipe.FoodString, isExitsUser.UserName, request.Name, 1);
                new_recipe.ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(new_recipe.FoodString, isExitsUser.UserName, request.Name, 2);
                new_recipe.ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(new_recipe.FoodString, isExitsUser.UserName, request.Name, 3);
                new_recipe.ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(new_recipe.FoodString, isExitsUser.UserName, request.Name, 4);
                new_recipe.ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(new_recipe.FoodString, isExitsUser.UserName, request.Name, 5);
            
            }
            _db.Recipes.Add(new_recipe);
            foreach (var item in ExistIngredients)
            {
                var isExitsIngredient = _db.Ingredients.Where(x => x.Name == item.Item1).FirstOrDefault();
                if (isExitsIngredient == null)
                {
                    Console.WriteLine("*************&&&&&&&&&&&&&&&**********(((((9");
                }
                RecipeIngredients row = new RecipeIngredients
                {
                    RecipeId = new_recipe.Id,
                    IngredientId = isExitsIngredient.Id,
                    Quantity = double.Parse(item.Item2),
                    Unit = item.Item3
                };
                _db.RecipeIngredients.Add(row);
            }
            foreach (var item in request.Steps)
            {
                RecipeStep row = new RecipeStep
                {
                    RecipeId = new_recipe.Id,
                    explenation = item.Item2,
                    Number = int.Parse(item.Item1)
                };
                _db.RecipeSteps.Add(row);
            }
            _db.SaveChanges();

            return new RecipeResponse()
            {
                IsSucceed = true,
                Message = "InCompeleteRecipe Added Successfully",
                recipe = new_recipe
            };
        }
        public async Task<RecipeResponse> DeleteRecipe(string FoodName, string UserName, string Name, string userId)
        {
            var Recipe = await Search_Recipe(FoodName, UserName, Name);
            if (Recipe != null)
            {
                _db.Recipes.Remove(Recipe);
                _db.SaveChanges();
                return new RecipeResponse()
                {
                    IsSucceed = true,
                    Message = "Recipe removed Successfully",
                    recipe = null
                };
            }
            Recipe = await Search_InComplete_Recipe(FoodName, UserName, Name);
            if (Recipe != null)
            {
                _db.Recipes.Remove(Recipe);
                _db.SaveChanges();
                return new RecipeResponse()
                {
                    IsSucceed = true,
                    Message = "Recipe removed Successfully",
                    recipe = null
                };
            }
            return new RecipeResponse()
            {
                IsSucceed = false,
                Message = "This Recipe Not exist",
                recipe = null
            };
        }
        public async Task<InterGeneralResponse> AcceptedRecipe(string FoodName, string UserName, string Name)
        {
            var isInCompelete = await Search_InComplete_Recipe(FoodName,UserName,Name);
            if (isInCompelete != null)
            {
                Console.WriteLine(";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;");
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = "This recipe is not complete"
                };
            }
            var isExistsRecipe = _db.Recipes.Where(x => x.food.Name == FoodName && x.chef.UserName == UserName && x.Name == Name).FirstOrDefault();
            if (isExistsRecipe == null)
            {
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = "Recipe Not Exist"
                };
            }
            isExistsRecipe.CreatTime = DateTime.Now;
            isExistsRecipe.IsAccepted = true;
            isExistsRecipe.IsReject = false;
            _db.Recipes.Update(isExistsRecipe);
            _db.SaveChanges();
            return new InterGeneralResponse
            {
                IsSucceed = true,
                Message = "Recipe accepted"
            };
        }
        public async Task<InterGeneralResponse> RejectedRecipe(string FoodName, string UserName, string Name)
        {
            var isExistsRecipe = _db.Recipes.Where(x => (x.food!=null && x.food.Name == FoodName||x.FoodString==FoodName) && x.chef.UserName == UserName && x.Name == Name).FirstOrDefault();
            if (isExistsRecipe == null)
            {
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = "Recipe Not Exist"
                };
            }
            isExistsRecipe.CreatTime = DateTime.Now;
            isExistsRecipe.IsAccepted = false;
            isExistsRecipe.IsReject = true;
            _db.Recipes.Update(isExistsRecipe);
            _db.SaveChanges();
            return new InterGeneralResponse
            {
                IsSucceed = true,
                Message = "Recipe rejected"
            };
        }
        public async Task<IEnumerable<Recipe>> Get_All_Recipe()
        {
            var Recipes = _db.Recipes.ToList();
            return Recipes;
        }
        public async Task<Recipe> Search_Recipe(string FoodName, string ChefName, string RecipeName)
        {
            var Recipe = _db.Recipes.Where(r => r.chef.UserName.ToLower() == ChefName.ToLower() &&r.FoodId!=null&& r.food.Name.ToLower() == FoodName.ToLower() && r.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
            return Recipe;
        }
        public async Task<Recipe> Search_InComplete_Recipe(string FoodName, string ChefName, string RecipeName)
        {
            var isExitsFood = _db.Foods.Where(x => x.Name == FoodName).FirstOrDefault();
            Recipe recipe=null;
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++==",isExitsFood==null?"true":"false");
            if (isExitsFood != null)
            {
                Console.WriteLine("hhduidhw+++++++++++++++++++++++++++++++++=hiwgikwgiywgeuwfugeiweeeeeeeeeeeeeeee");
                recipe = _db.Recipes.Where(r => r.chef.UserName.ToLower() == ChefName.ToLower() && isExitsFood.Name.ToLower() == FoodName.ToLower() && r.NotExistIngredients != "" && r.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
                if (recipe == null)
                {
                    return null;
                }
            }
            else
            {
                Console.WriteLine("((((((((((((((((((((((((((((((((((*************");
                recipe = _db.Recipes.Where(r => r.chef.UserName.ToLower() == ChefName.ToLower() && r.FoodString.ToLower()== FoodName.ToLower()  && r.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
                if (recipe == null)
                {
                    return null;
                }
            }
            Console.WriteLine("{[[[[[[[[[[[[[[[[[[[[[[[[[");
            return recipe;
        }
        public async Task<InterGeneralResponse> RateRecipe(Recipe recipe, int rate)
        {
            recipe.Score = (recipe.Score * recipe.Number_Scorer + rate) / (recipe.Number_Scorer + 1);
            recipe.Number_Scorer += 1;
            _db.Recipes.Update(recipe);
            _db.SaveChanges();
            return new InterGeneralResponse() { Message = "Rating Succesfully", IsSucceed = true };
        }
        public async Task<InterGeneralResponse> DeleteRateRecipe(Recipe recipe, int rate)
        {
            if (recipe.Score > 1)
            {
                recipe.Score = (recipe.Score * recipe.Number_Scorer - rate) / (recipe.Number_Scorer - 1);
            }
            else
            {
                recipe.Score = 0;
            }
            recipe.Number_Scorer -= 1;
            _db.Recipes.Update(recipe);
            _db.SaveChanges();
            return new InterGeneralResponse() { Message = "Delete Rate Succesfully", IsSucceed = true };
        }
        public async Task<IEnumerable<Ingredient>> Get_All_Ingredients(string FoodName, string ChefName, string RecipeName)
        {
            var Ingredients = _db.RecipeIngredients.Where(r => r.recipe.chef.UserName.ToLower() == ChefName.ToLower() && r.recipe.food.Name.ToLower() == FoodName.ToLower() && r.recipe.Name.ToLower() == RecipeName.ToLower()).Select(r => r.ingredient).ToList();
            return Ingredients;
        }
        public async Task<IEnumerable<RecipeStep>> Get_All_steps(string FoodName, string ChefName, string RecipeName)
        {
            var Steps = _db.RecipeSteps.Where(r => r.recipe.chef.UserName.ToLower() == ChefName.ToLower() && r.recipe.food.Name.ToLower() == FoodName.ToLower() && r.recipe.Name.ToLower() == RecipeName.ToLower()).ToList();
            return Steps;
        }
    }
}
