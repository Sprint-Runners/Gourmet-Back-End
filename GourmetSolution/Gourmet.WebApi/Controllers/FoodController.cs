using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Requests.BatchRequest;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<Chef> _userManager;
        private readonly IUserService _userService;
        private readonly AppDbContext _db;
        private readonly IImageProcessorService _imageProcessorService;
        public FoodController(IFoodService foodService, IRecipeService recipeService, UserManager<Chef> userManager, IUserService userService, AppDbContext db, IImageProcessorService imageProcessorService)
        {
            _foodService = foodService;
            _recipeService = recipeService;
            _userManager = userManager;
            _userService = userService;
            _db = db;
            _imageProcessorService = imageProcessorService;
        }
        [HttpPut("LikeRecipesByUser")]
        [Authorize]
        public async Task<IActionResult> LikeRecipeByUser(FavouritRecentRecipeRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            InterGeneralResponse response = await _userService.AddFavouritRecipeForUser(user, request.FoodName, request.ChefName, request.RecipeName);
            if (response.IsSucceed)
            {
                return Ok(new GeneralResponse { Message = response.Message });
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpPut("RateRecipesByUser")]
        [Authorize]
        public async Task<IActionResult> RateRecipeByUser(RatingRecipeRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            InterGeneralResponse response = await _userService.AddScoreRecipeForUser(user, request.FoodName, request.ChefName, request.RecipeName, request.rate);
            if (response.IsSucceed)
            {
                return Ok(new GeneralResponse { Message = response.Message });
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpPut("DeleteRateRecipesByUser")]
        [Authorize]
        public async Task<IActionResult> DeleteRateRecipeByUser(RatingRecipeRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            InterGeneralResponse response = await _userService.DeleteScoreRecipeForUser(user, request.FoodName, request.ChefName, request.RecipeName, request.rate);
            if (response.IsSucceed)
            {
                return Ok(new GeneralResponse { Message = response.Message });
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpPut("VisitRecipesByUser")]
        [Authorize]
        public async Task<IActionResult> VisitRecipeByUser(FavouritRecentRecipeRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            InterGeneralResponse response = await _userService.AddRecentRecipeForUser(user, request.FoodName, request.ChefName, request.RecipeName);
            if (response.IsSucceed)
            {
                return Ok(new GeneralResponse { Message = response.Message });
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpPut("AllRecipesUser")]
        [Authorize]
        public async Task<IActionResult> AllRecipesUser(ShowFoodpageRequest request)
        {
            string FoodName = request.FoodName;
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            var response = await _foodService.GetAllRecipe(FoodName);
            List<RecipeInformationForUserResponse> results = new List<RecipeInformationForUserResponse>();
            response.Recipes = response.Recipes.OrderByDescending(r => r.Score).ToList();
            if (response.IsSucceed)
            {
                //var isExitsUserFirst = await _userManager.FindByIdAsync(response.Recipes.First().ChefId);
                //InterGeneralResponse AddRecentRecipe = await _userService.AddRecentRecipeForUser(user, FoodName, isExitsUserFirst.UserName, response.Recipes.First().Name);
                //if (AddRecentRecipe.IsSucceed)
                //{
                    foreach (var item in response.Recipes)
                    {
                        var isExitsUser = await _userManager.FindByIdAsync(item.ChefId);
                        var ingredients = await _recipeService.Get_All_Ingredients(FoodName, isExitsUser.UserName, item.Name);
                        var steps = await _recipeService.Get_All_steps(FoodName, isExitsUser.UserName, item.Name);
                        var IsRateforthisRecipe = _db.ScoreRecipeUsers.Where(r => r.userId == user.Id && r.RecipeId == item.Id).FirstOrDefault();
                        var IsLikethisRecipe = _db.FavouritRecipeUsers.Where(r => r.userId == user.Id && r.RecipeId == item.Id).FirstOrDefault();
                        int rate = 0;
                        if (IsRateforthisRecipe != null)
                        {
                            rate = IsRateforthisRecipe.Rate;
                        }
                        var allPSOI = _db.PSOIs.ToList();
                        var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                        var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                        var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                        var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                        var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                        var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                        results.Add(new RecipeInformationForUserResponse
                        {
                            Name = item.Name,
                            FoodName = FoodName,
                            ChefName = isExitsUser.FullName,
                            ChefUserName = isExitsUser.UserName,
                            ChefImageUrl = await _imageProcessorService.GetImagebyUser(isExitsUser.UserName),
                            Description = item.Description,
                            Score = item.Score,
                            ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 1),
                            ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 2),
                            ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 3),
                            ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 4),
                            ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 5),
                            List_Ingriedents = ingredients.ToList(),
                            PSOIName = isExitsPSOI.Name,
                            CMName = isExitsCM.Name,
                            FTName = isExitsFT.Name,
                            NName = isExitsN.Name,
                            MTName = isExitsMT.Name,
                            DLName = isExistDL.Name,
                            time = item.Time,
                            Steps = steps.OrderBy(r => r.Number).ToList(),
                            Rate = rate,
                            isFavourit = IsLikethisRecipe != null,
                            isRate = IsRateforthisRecipe != null,
                            CountRate = item.Number_Scorer,
                        });
                    }

                    return Ok(results);
                //}
                //return Problem(detail: AddRecentRecipe.Message, statusCode: 400);
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpPut("AllRecipes")]
        public async Task<IActionResult> AllRecipes(ShowFoodpageRequest request)
        {
            string FoodName = request.FoodName;
            var response = await _foodService.GetAllRecipe(FoodName);
            List<RecipeInformationResponse> results = new List<RecipeInformationResponse>();
            if (response.IsSucceed)
            {
                foreach (var item in response.Recipes.OrderByDescending(r => r.Score))
                {

                    var isExitsUser = await _userManager.FindByIdAsync(item.ChefId);
                    var ingredients = await _recipeService.Get_All_Ingredients(FoodName, isExitsUser.UserName, item.Name);
                    var steps = await _recipeService.Get_All_steps(FoodName, isExitsUser.UserName, item.Name);
                    Console.WriteLine("djffefuy*************");
                    Console.WriteLine(results);
                    var allPSOI = _db.PSOIs.ToList();
                    var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                    var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                    var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                    var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                    var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                    var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                    results.Add(new RecipeInformationResponse
                    {
                        Name = item.Name,
                        FoodName = FoodName,
                        ChefName = isExitsUser.FullName,
                        ChefUserName = isExitsUser.UserName,
                        ChefImageUrl = await _imageProcessorService.GetImagebyUser(isExitsUser.UserName),
                        Description = item.Description,
                        Score = item.Score,
                        ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 1),
                        ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 2),
                        ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 3),
                        ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 4),
                        ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(FoodName, isExitsUser.UserName, item.Name, 5),
                        List_Ingriedents = ingredients.ToList(),
                        PSOIName = isExitsPSOI.Name,
                        CMName = isExitsCM.Name,
                        FTName = isExitsFT.Name,
                        NName = isExitsN.Name,
                        MTName = isExitsMT.Name,
                        DLName = isExistDL.Name,
                        time = item.Time,
                        Steps = steps.OrderBy(r => r.Number).ToList(),
                        CountRate = item.Number_Scorer,
                    });
                }
                return Ok(results);
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpPut("SpecificRecipe")]
        public async Task<IActionResult> SpecificRecipes(ShowRecipepageRequest request)
        {
            var recipe = await _recipeService.Search_Recipe(request.FoodName, request.ChefName, request.RecipeName);
            RecipeInformationResponse result = new RecipeInformationResponse();

            var isExitsUser = await _userManager.FindByIdAsync(recipe.ChefId);
            var ingredients = await _recipeService.Get_All_Ingredients(request.FoodName, isExitsUser.UserName, recipe.Name);
            var steps = await _recipeService.Get_All_steps(request.FoodName, isExitsUser.UserName, recipe.Name);
            var allPSOI = _db.PSOIs.ToList();
            var isExitsPSOI = allPSOI.Where(x => x.Id == recipe.Primary_Source_of_IngredientId).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Id == recipe.Cooking_MethodId).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Id == recipe.Food_typeId).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Id == recipe.NationalityId).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Id == recipe.Meal_TypeId).FirstOrDefault();
            var isExistDL = _db.DLs.Where(x => x.Id == recipe.Difficulty_LevelId).FirstOrDefault();
            result = new RecipeInformationResponse
            {
                Name = recipe.Name,
                FoodName = request.FoodName,
                ChefName = isExitsUser.FullName,
                ChefUserName = isExitsUser.UserName,
                ChefImageUrl = await _imageProcessorService.GetImagebyUser(isExitsUser.UserName),
                Description = recipe.Description,
                Score = recipe.Score,
                ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 1),
                ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 2),
                ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 3),
                ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 4),
                ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 5),
                List_Ingriedents = ingredients.ToList(),
                PSOIName = isExitsPSOI.Name,
                CMName = isExitsCM.Name,
                FTName = isExitsFT.Name,
                NName = isExitsN.Name,
                MTName = isExitsMT.Name,
                DLName = isExistDL.Name,
                time = recipe.Time,
                Steps = steps.OrderBy(r => r.Number).ToList(),
                CountRate = recipe.Number_Scorer,
            };
            return Ok(result);
        }
        [HttpPut("SpecificRecipeUser")]
        [Authorize]
        public async Task<IActionResult> SpecificRecipesUser(ShowRecipepageRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            var recipe = await _recipeService.Search_Recipe(request.FoodName, request.ChefName, request.RecipeName);
            RecipeInformationForUserResponse result = new RecipeInformationForUserResponse();

            var isExitsUser = await _userManager.FindByIdAsync(recipe.ChefId);
            var ingredients = await _recipeService.Get_All_Ingredients(request.FoodName, isExitsUser.UserName, recipe.Name);
            InterGeneralResponse AddRecentRecipe = await _userService.AddRecentRecipeForUser(user, request.FoodName, isExitsUser.UserName, recipe.Name);
            var steps = await _recipeService.Get_All_steps(request.FoodName, isExitsUser.UserName, recipe.Name);
            var IsRateforthisRecipe = _db.ScoreRecipeUsers.Where(r => r.userId == user.Id && r.RecipeId == recipe.Id).FirstOrDefault();
            var IsLikethisRecipe = _db.FavouritRecipeUsers.Where(r => r.userId == user.Id && r.RecipeId == recipe.Id).FirstOrDefault();
            int rate = 0;
            if (IsRateforthisRecipe != null)
            {
                rate = IsRateforthisRecipe.Rate;
            }
            var allPSOI = _db.PSOIs.ToList();
            var isExitsPSOI = allPSOI.Where(x => x.Id == recipe.Primary_Source_of_IngredientId).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Id == recipe.Cooking_MethodId).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Id == recipe.Food_typeId).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Id == recipe.NationalityId).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Id == recipe.Meal_TypeId).FirstOrDefault();
            var isExistDL = _db.DLs.Where(x => x.Id == recipe.Difficulty_LevelId).FirstOrDefault();
            result = new RecipeInformationForUserResponse
            {
                Name = recipe.Name,
                FoodName = request.FoodName,
                ChefName = isExitsUser.FullName,
                ChefUserName = isExitsUser.UserName,
                ChefImageUrl = await _imageProcessorService.GetImagebyUser(isExitsUser.UserName),
                Description = recipe.Description,
                Score = recipe.Score,
                ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 1),
                ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 2),
                ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 3),
                ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 4),
                ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(request.FoodName, isExitsUser.UserName, recipe.Name, 5),
                List_Ingriedents = ingredients.ToList(),
                PSOIName = isExitsPSOI.Name,
                CMName = isExitsCM.Name,
                FTName = isExitsFT.Name,
                NName = isExitsN.Name,
                MTName = isExitsMT.Name,
                DLName = isExistDL.Name,
                time = recipe.Time,
                Steps = steps.OrderBy(r => r.Number).ToList(),
                Rate = rate,
                isFavourit = IsLikethisRecipe != null,
                isRate = IsRateforthisRecipe != null,
                CountRate = recipe.Number_Scorer,
            };
            return Ok(result);
        }
        //[HttpGet("Special")]
        //public async Task<IActionResult> Get_Special_Foods()
        //{
        //    var response = await _foodService.Get_Special();
        //    return Ok(response);
        //}
    }
}
