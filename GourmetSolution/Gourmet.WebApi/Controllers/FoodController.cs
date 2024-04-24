using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IRecipeService _recipeService;
        public FoodController(IFoodService foodService, IRecipeService recipeService)
        {
            _foodService = foodService;
            _recipeService = recipeService;
        }
        [HttpGet("AllRecipes")]
        public async Task<IActionResult> AllRecipes(string FoodName)
        {
            var response = await _foodService.GetAllRecipe(FoodName);
            List<RecipeInformationResponse> results = new List<RecipeInformationResponse>();
            if (response.IsSucceed)
            {
                foreach (var item in response.Recipes)
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
                        ImgeUrl = item.ImgeUrl,
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
    }
}
