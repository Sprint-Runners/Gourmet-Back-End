using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IFoodService _foodservice;
        private readonly IRecipeService _recipeservice;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly UserManager<Chef> _userManager;
        public MenuController(AppDbContext db, IFoodService foodService, IImageProcessorService imageProcessorService, IRecipeService recipeservice, UserManager<Chef> userManager)
        {
            _imageProcessorService = imageProcessorService;
            _recipeservice = recipeservice;
            _foodservice = foodService;
            _db = db;
            _userManager = userManager;
        }
        [HttpGet("Menu")]
        public async Task<IActionResult> MenuAsync()
        {

            List<Food> foods = await _foodservice.Get_All();
            List<FoodInformationResponse> allfoods = new List<FoodInformationResponse>();
            foreach (Food row in foods)
            {
                row.ImgeUrl = await _imageProcessorService.GetImagebyFood(row.Name);
                allfoods.Add(new FoodInformationResponse
                {
                    Name = row.Name,
                    ImagePath = row.ImgeUrl,
                }); ;
            }
            var AllRecipes = _db.Recipes.ToList();
            AllRecipes = AllRecipes.Where(r => r.IsAccepted == true && r.IsReject == false).ToList();
            var quicks = AllRecipes.Where(r => r.Time <= 30).ToList();

            Meal_Type breakfast = _db.MTs.Where(r => r.Name == "Breakfast").FirstOrDefault();
            Console.WriteLine("im here$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$*****"+breakfast);
            var breakfasts = AllRecipes.Where(r => r.Meal_TypeId == breakfast.Id).ToList();
            Console.WriteLine("im here**********************************************");
            Primary_Source_of_Ingredient vegan = _db.PSOIs.Where(r => r.Name == "Vegan").FirstOrDefault();
            var vegans = AllRecipes.Where(r => r.Primary_Source_of_IngredientId == vegan.Id).ToList();
            Meal_Type dinner = _db.MTs.Where(r => r.Name == "Dinner").FirstOrDefault();
            Meal_Type lunch = _db.MTs.Where(r => r.Name == "Lunch").FirstOrDefault();
            var mains = AllRecipes.Where(r => r.Meal_TypeId == dinner.Id || r.Meal_TypeId == lunch.Id).ToList();
            Console.WriteLine("im here***********************************111111111111111111***********");
            var bestfoods = AllRecipes.Where(r => r.Score >= 4.0).ToList();
            Console.WriteLine("im here**************************************2222222222222222222222222222********");
            Dictionary<string, List<SummaryRecipeInfoResponse>> reciperesult = new Dictionary<string, List<SummaryRecipeInfoResponse>>();

            List<SummaryRecipeInfoResponse> quicksRecipe = new List<SummaryRecipeInfoResponse>();
            List<SummaryRecipeInfoResponse> vegansRecipe = new List<SummaryRecipeInfoResponse>();
            List<SummaryRecipeInfoResponse> mainsRecipe = new List<SummaryRecipeInfoResponse>();
            List<SummaryRecipeInfoResponse> bestfoodsRecipe = new List<SummaryRecipeInfoResponse>();
            List<SummaryRecipeInfoResponse> breakfastsRecipe = new List<SummaryRecipeInfoResponse>();
            foreach (var item in quicks)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var isExitschef = await _userManager.FindByIdAsync(item.ChefId);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                quicksRecipe.Add(new SummaryRecipeInfoResponse
                {
                    Score = item.Score,
                    ChefName = isExitschef.FullName,
                    Description = item.Description,
                    ChefUserName = isExitschef.UserName,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitschef.UserName, item.Name, 1),
                    Name = item.Name,
                    FoodName = isExitsFood.Name,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer,
                });

            }
            reciperesult.Add("Quick", quicksRecipe);
            foreach (var item in vegans)
            {
                var isExitschef = await _userManager.FindByIdAsync(item.ChefId);
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                vegansRecipe.Add(new SummaryRecipeInfoResponse
                {
                    Score = item.Score,
                    ChefName = isExitschef.FullName,
                    Description = item.Description,
                    ChefUserName = isExitschef.UserName,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitschef.UserName, item.Name, 1),
                    Name = item.Name,
                    FoodName = isExitsFood.Name,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer
                }) ;

            }
            reciperesult.Add("Vegan", vegansRecipe);
            foreach (var item in mains)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var isExitschef = await _userManager.FindByIdAsync(item.ChefId);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                mainsRecipe.Add(new SummaryRecipeInfoResponse
                {
                    Score = item.Score,
                    ChefName = isExitschef.FullName,
                    ChefUserName = isExitschef.UserName,
                    Description = item.Description,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitschef.UserName, item.Name, 1),
                    Name = item.Name,
                    FoodName = isExitsFood.Name,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer,
                });

            }
            reciperesult.Add("Main", mainsRecipe);
            foreach (var item in breakfasts)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var isExitschef = await _userManager.FindByIdAsync(item.ChefId);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                breakfastsRecipe.Add(new SummaryRecipeInfoResponse
                {
                    Score = item.Score,
                    ChefName = isExitschef.FullName,
                    ChefUserName = isExitschef.UserName,
                    Description = item.Description,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitschef.UserName, item.Name, 1),
                    Name = item.Name,
                    FoodName = isExitsFood.Name,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer,
                });

            }
            reciperesult.Add("Breakfast", breakfastsRecipe);
            foreach (var item in bestfoods)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var isExitschef = await _userManager.FindByIdAsync(item.ChefId);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                bestfoodsRecipe.Add(new SummaryRecipeInfoResponse
                {
                    Score = item.Score,
                    ChefName = isExitschef.FullName,
                    ChefUserName = isExitschef.UserName,
                    Description = item.Description,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitschef.UserName, item.Name, 1),
                    Name = item.Name,
                    FoodName = isExitsFood.Name,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer,
                });

            }
            reciperesult.Add("Best", bestfoodsRecipe);
            Tuple<List<FoodInformationResponse>, Dictionary<string, List<SummaryRecipeInfoResponse>>> result = new Tuple<List<FoodInformationResponse>, Dictionary<string, List<SummaryRecipeInfoResponse>>>(allfoods, reciperesult);
            return Ok(result);

        }
    }
}

