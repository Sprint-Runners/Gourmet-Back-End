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
        [HttpPut("InformaionOfChef")]
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
                    var isExitsFood = _db.Foods.Where(x => x.Id==r.FoodId).FirstOrDefault();
                    var isExistDL = _db.DLs.Where(x => x.Id==r.Difficulty_LevelId).FirstOrDefault();
                    r.ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExistsChef.UserName, r.Name,1);
                    var allPSOI = _db.PSOIs.ToList();
                    var isExitsPSOI = allPSOI.Where(x => x.Id == r.Primary_Source_of_IngredientId).FirstOrDefault();
                    var isExitsCM = _db.CMs.Where(x => x.Id == r.Cooking_MethodId).FirstOrDefault();
                    var isExitsFT = _db.FTs.Where(x => x.Id == r.Food_typeId).FirstOrDefault();
                    var isExitsN = _db.Ns.Where(x => x.Id == r.NationalityId).FirstOrDefault();
                    var isExitsMT = _db.MTs.Where(x => x.Id == r.Meal_TypeId).FirstOrDefault();
                    TopChefRecipeResult.Add(new SummaryRecipeInfoResponse
                    {
                        FoodName = isExitsFood.Name,
                        Name = r.Name,
                        ChefName = isExistsChef.FullName,
                        ChefUserName = isExistsChef.UserName,
                        ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExistsChef.UserName, r.Name, 1),
                        Score = r.Score,
                        Description = r.Description,
                        Time = r.Time,
                        CMName = isExitsCM.Name,
                        DLName = isExistDL.Name,
                        FTName = isExitsFT.Name,
                        MTName = isExitsMT.Name,
                        NName = isExitsN.Name,
                        PSOIName = isExitsPSOI.Name,
                        CountRate = r.Number_Scorer
                    });
                }
                foreach (Recipe r in LastChefRecipe)
                {
                    var isExitsFood = _db.Foods.Where(x => x.Id == r.FoodId).FirstOrDefault();
                    var isExistDL = _db.DLs.Where(x => x.Id == r.Difficulty_LevelId).FirstOrDefault();
                    r.ImgeUrl1 = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExistsChef.UserName, r.Name,1);
                    var allPSOI = _db.PSOIs.ToList();
                    var isExitsPSOI = allPSOI.Where(x => x.Id == r.Primary_Source_of_IngredientId).FirstOrDefault();
                    var isExitsCM = _db.CMs.Where(x => x.Id == r.Cooking_MethodId).FirstOrDefault();
                    var isExitsFT = _db.FTs.Where(x => x.Id == r.Food_typeId).FirstOrDefault();
                    var isExitsN = _db.Ns.Where(x => x.Id == r.NationalityId).FirstOrDefault();
                    var isExitsMT = _db.MTs.Where(x => x.Id == r.Meal_TypeId).FirstOrDefault();
                    LastChefRecipeResult.Add(new SummaryRecipeInfoResponse
                    {
                        FoodName = isExitsFood.Name,
                        Name = r.Name,
                        ChefName = isExistsChef.FullName,
                        ChefUserName = isExistsChef.UserName,
                        ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name,isExistsChef.UserName,r.Name,1),
                        Score = r.Score,
                        Description = r.Description,
                        Time = r.Time,
                        CMName = isExitsCM.Name,
                        DLName = isExistDL.Name,
                        FTName = isExitsFT.Name,
                        MTName = isExitsMT.Name,
                        NName = isExitsN.Name,
                        PSOIName = isExitsPSOI.Name,
                        CountRate = r.Number_Scorer
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
