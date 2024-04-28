using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly IChefService _chefservice;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly UserManager<Chef> _userManager;
        public ChefController(AppDbContext db, IChefService chefService, IImageProcessorService imageProcessorService, UserManager<Chef> userManager)
        {
            _db = db;
            _chefservice = chefService;
            _imageProcessorService = imageProcessorService;
            _userManager = userManager;
        }
        [HttpGet("InformaionOfChef")]
        public async Task<IActionResult> InformaionOfChef(GetInformationChefRequest request)
        {
            try
            {
                var isExistsChef = await _userManager.FindByNameAsync(request.ChefUserName);
                if (isExistsChef == null)
                {
                    return Problem(detail: "Chef not exist", statusCode: 400);
                }

                var TopChefRecipes = await _chefservice.GetAcceptedRecipesByChefId(isExistsChef.Id);
                TopChefRecipes = TopChefRecipes.OrderByDescending(r => r.Score).ToList();
                var LastChefRecipe = await _chefservice.GetAcceptedRecipesByChefId(isExistsChef.Id);
                LastChefRecipe = LastChefRecipe.OrderByDescending(r => r.CreatTime).ToList();
                List<SummaryRecipeInfoResponse> TopChefRecipeResult = new List<SummaryRecipeInfoResponse>();
                List<SummaryRecipeInfoResponse> LastChefRecipeResult = new List<SummaryRecipeInfoResponse>();
                foreach (Recipe r in TopChefRecipes)
                {
                    r.ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(r.food.Name, r.chef.UserName, r.Name,1);
                    TopChefRecipeResult.Add(new SummaryRecipeInfoResponse
                    {
                        FoodName = r.food.Name,
                        Name = r.Name,
                        ChefName = r.chef.FullName,
                        ChefUserName = r.chef.UserName,
                        ImagePath = r.ImgeUrl1,
                        Score = r.Score,
                        Description = r.Description,
                        DifficultyLevel = r.difficulty_Level.Name,
                        Time = r.Time
                    });
                }
                foreach (Recipe r in LastChefRecipe)
                {
                    r.ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(r.food.Name, r.chef.UserName, r.Name,1);
                    LastChefRecipeResult.Add(new SummaryRecipeInfoResponse
                    {
                        FoodName = r.food.Name,
                        Name = r.Name,
                        ChefName = r.chef.FullName,
                        ChefUserName = r.chef.UserName,
                        ImagePath = r.ImgeUrl1,
                        Score = r.Score,
                        Description = r.Description,
                        DifficultyLevel = r.difficulty_Level.Name,
                        Time = r.Time
                    });
                }
                return Ok(new ShowChefInformationResponse
                {
                    FullName = isExistsChef.FullName,
                    UserName = isExistsChef.UserName,
                    Score = isExistsChef.Score,
                    Aboutme = isExistsChef.Aboutme,
                    ImageURL = await _imageProcessorService.GetImagebyUser(isExistsChef.UserName),
                    LastRecipes = LastChefRecipeResult,
                    TopRecipes = TopChefRecipeResult,
                    PhoneNumber = isExistsChef.PhoneNumber,
                    RecipeCount=LastChefRecipeResult.Count
                });
            
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
    }
}
