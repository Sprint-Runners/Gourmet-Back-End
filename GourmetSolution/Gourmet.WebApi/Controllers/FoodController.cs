using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            InterGeneralResponse response = await _userService.AddScoreRecipeForUser(user, request.FoodName, request.ChefName, request.RecipeName,request.rate);
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
        public async Task<IActionResult> AllRecipesUser(string FoodName)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            var response = await _foodService.GetAllRecipe(FoodName);
            List<RecipeInformationForUserResponse> results = new List<RecipeInformationForUserResponse>();
            response.Recipes = response.Recipes.OrderByDescending(r => r.Score).ToList();
            if (response.IsSucceed)
            {
                InterGeneralResponse AddRecentRecipe = await _userService.AddRecentRecipeForUser(user, FoodName, response.Recipes.First().chef.UserName, response.Recipes.First().Name);
                if (AddRecentRecipe.IsSucceed)
                {
                    foreach (var item in response.Recipes)
                    {
                        
                        var ingredients = await _recipeService.Get_All_Ingredients(FoodName, item.chef.UserName, item.Name);
                        var steps = await _recipeService.Get_All_steps(FoodName, item.chef.UserName, item.Name);
                        var IsRateforthisRecipe = _db.ScoreRecipeUsers.Where(r => r.userId == user.Id && r.RecipeId == item.Id).FirstOrDefault();
                        var IsLikethisRecipe = _db.FavouritRecipeUsers.Where(r => r.userId == user.Id && r.RecipeId == item.Id).FirstOrDefault();
                        int rate = 0;
                        if (IsRateforthisRecipe != null)
                        {
                            rate = IsRateforthisRecipe.Rate;
                        }
                        results.Add(new RecipeInformationForUserResponse
                        {
                            Name = item.Name,
                            FoodName = item.food.Name,
                            ChefName = item.chef.FullName,
                            ChefUserName = item.chef.UserName,
                            ChefImageUrl = item.chef.ImageURL,
                            Description = item.Description,
                            Score = item.Score,
                            ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(item.food.Name,item.chef.UserName,item.Name,1),
                            ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 2),
                            ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 3),
                            ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 4),
                            ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 5),
                            List_Ingriedents = ingredients.ToList(),
                            PSOIName = item.primary_source_of_ingredient.Name,
                            CMName = item.cooking_method.Name,
                            FTName = item.food_type.Name,
                            NName = item.nationality.Name,
                            MTName = item.meal_type.Name,
                            DLName = item.difficulty_Level.Name,
                            time = item.Time,
                            Steps = steps.OrderBy(r => r.Number).ToList(),
                            Rate = rate,
                            isFavourit=IsLikethisRecipe!=null,
                            isRate=IsRateforthisRecipe!=null
                        }) ;
                    }

                    return Ok(results);
                }
                return Problem(detail: AddRecentRecipe.Message, statusCode: 400);
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpGet("AllRecipes")]
        public async Task<IActionResult> AllRecipes(string FoodName)
        {
            var response = await _foodService.GetAllRecipe(FoodName);
            List<RecipeInformationResponse> results = new List<RecipeInformationResponse>();
            if (response.IsSucceed)
            {
                foreach (var item in response.Recipes.OrderByDescending(r => r.Score))
                {
                    var ingredients = await _recipeService.Get_All_Ingredients(FoodName, item.chef.UserName, item.Name);
                    var steps = await _recipeService.Get_All_steps(FoodName, item.chef.UserName, item.Name);
                    results.Add(new RecipeInformationResponse
                    {
                        Name = item.Name,
                        FoodName = item.food.Name,
                        ChefName = item.chef.FullName,
                        ChefImageUrl = item.chef.ImageURL,
                        Description = item.Description,
                        Score = item.Score,
                        ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 1),
                        ImgeUrl2 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 2),
                        ImgeUrl3 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 3),
                        ImgeUrl4 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 4),
                        ImgeUrl5 = await _imageProcessorService.GetImagebyRecipe(item.food.Name, item.chef.UserName, item.Name, 5),
                        List_Ingriedents = ingredients.ToList(),
                        PSOIName = item.primary_source_of_ingredient.Name,
                        CMName = item.cooking_method.Name,
                        FTName = item.food_type.Name,
                        NName = item.nationality.Name,
                        MTName = item.meal_type.Name,
                        DLName = item.difficulty_Level.Name,
                        time = item.Time,
                        Steps = steps.OrderBy(r => r.Number).ToList(),
                    });
                }
                return Ok(results);
            }
            return Problem(detail: response.Message, statusCode: 400);
        }
        [HttpGet("Special")]
        public async Task<IActionResult> Get_Special_Foods()
        {
            var response = await _foodService.Get_Special();
            return Ok(response);
        }
    }
}
