using FuzzySharp;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IRecipeService _recipeServicel;
        private readonly UserManager<Chef> _userManager;
        private readonly IImageProcessorService _imageProcessorService;
        public SearchController(AppDbContext db,IRecipeService recipeService,UserManager<Chef>userManager,IImageProcessorService imageProcessorService) 
        { 
            _db = db;
            _recipeServicel = recipeService;
            _userManager = userManager;
            _imageProcessorService = imageProcessorService;
        }
        [HttpPut("FullSearch")]
        public async Task<IActionResult> FullSearch(FullSearchRequest request)
        {
            var searchResults=new List<SearchRecipe>();
            List<SummaryRecipeInfoResponse> result = new List<SummaryRecipeInfoResponse>();
            searchResults = searchResults.Where(item => item.Recipe.Time <= request.Time).ToList();
            string format = "MM/dd/yyyy";
            DateTime startdate;
            DateTime enddate;
            if (!DateTime.TryParseExact(request.StartDate, format, null, System.Globalization.DateTimeStyles.None, out startdate))
            {
                return Problem(detail: "StartDate not correct", statusCode: 400);
            }
            if (!DateTime.TryParseExact(request.EndDate, format, null, System.Globalization.DateTimeStyles.None, out enddate))
            {
                return Problem(detail: "EndDate not correct", statusCode: 400);
            }
            enddate.AddDays(1);
            searchResults = searchResults.Where(item => item.Recipe.CreatTime >= startdate && item.Recipe.CreatTime <= enddate).ToList();

            if (request.Text != "")
            {
                request.Text = request.Text.ToLower().Trim();
                var AllRecipes = _db.Recipes.ToList();
                searchResults = AllRecipes.Select(obj => new SearchRecipe
                {
                    Recipe = obj,
                    PartialRatioScore = Fuzz.PartialRatio(obj.Name.ToLower().Trim(), request.Text),
                    RatioScore = Fuzz.Ratio(obj.Name.ToLower().Trim(), request.Text)
                }).Where(result => result.PartialRatioScore >= 50 || result.RatioScore >= 70)
                .OrderByDescending(result => Math.Max(result.PartialRatioScore, result.RatioScore))
                .ToList();
            }
            else
            {
                var AllRecipes = _db.Recipes.ToList();
                searchResults = AllRecipes.Select(obj => new SearchRecipe
                {
                    Recipe = obj,
                    PartialRatioScore = 100,
                    RatioScore = 100
                })
                .ToList();
            }
            if (request.PSOI.Count > 0)
            {
                var AllPSOIs = _db.PSOIs.Where(item => request.PSOI.Any(name => item.Name == name)).Select(item => item.Id);
                searchResults = searchResults.Where(item => AllPSOIs.Any(id => item.Recipe.Primary_Source_of_IngredientId == id)).ToList();
            }
            if (request.CM.Count > 0)
            {
                var AllCMs = _db.CMs.Where(item => request.CM.Any(name => item.Name == name)).Select(item => item.Id);
                searchResults = searchResults.Where(item => AllCMs.Any(id => item.Recipe.Cooking_MethodId == id)).ToList();
            }
            if (request.FT.Count > 0)
            {
                var AllFTs = _db.FTs.Where(item => request.FT.Any(name => item.Name == name)).Select(item => item.Id);
                searchResults = searchResults.Where(item => AllFTs.Any(id => item.Recipe.Food_typeId == id)).ToList();
            }
            if (request.MT.Count > 0)
            {
                var AllMTs = _db.MTs.Where(item => request.MT.Any(name => item.Name == name)).Select(item => item.Id);
                searchResults = searchResults.Where(item => AllMTs.Any(id => item.Recipe.Meal_TypeId == id)).ToList();
            }
            if (request.DL.Count > 0)
            {
                var AllDLs = _db.DLs.Where(item => request.DL.Any(name => item.Name == name)).Select(item => item.Id);
                searchResults = searchResults.Where(item => AllDLs.Any(id => item.Recipe.Difficulty_LevelId == id)).ToList();
            }
            if (request.N.Count > 0)
            {
                var AllNs = _db.Ns.Where(item => request.N.Any(name => item.Name == name)).Select(item => item.Id);
                searchResults = searchResults.Where(item => AllNs.Any(id => item.Recipe.NationalityId == id)).ToList();
            }
            var newSearchResult = new List<SearchRecipe>();
            if (request.ingredients.Count > 0) {
                foreach (var item in searchResults)
                {
                    int count = 0;
                    var isExitschef = await _userManager.FindByIdAsync(item.Recipe.ChefId);
                    var isExitsFood = _db.Foods.Where(x => x.Id == item.Recipe.FoodId).FirstOrDefault();
                    var ings = await _recipeServicel.Get_All_Ingredients(isExitsFood.Name, isExitschef.UserName, item.Recipe.Name);
                    foreach (var ing in request.ingredients)
                    {
                        if (ings.Where(ingr => ingr.Name == ing).FirstOrDefault() != null)
                        {
                            count++;
                        }
                    }
                    if ((count>=3 && request.ingredients.Count==4) || (count >= 2 && request.ingredients.Count == 3) || (count == 2 && request.ingredients.Count == 2) || (count == 1 && request.ingredients.Count == 1))
                    {
                        newSearchResult.Add(item);
                    }
                }
            }
            else
            {
                newSearchResult = searchResults;
            }
            foreach(var item in newSearchResult)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.Recipe.FoodId).FirstOrDefault();
                var isExitschef = await _userManager.FindByIdAsync(item.Recipe.ChefId);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Recipe.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Recipe.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Recipe.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.Recipe.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Recipe.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Recipe.Difficulty_LevelId).FirstOrDefault();
                result.Add(new SummaryRecipeInfoResponse
                {
                    Score = item.Recipe.Score,
                    ChefName = isExitschef.FullName,
                    Description = item.Recipe.Description,
                    ChefUserName = isExitschef.UserName,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExitschef.UserName, item.Recipe.Name, 1),
                    Name = item.Recipe.Name,
                    FoodName = isExitsFood.Name,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    PSOIName = isExitsPSOI.Name,
                    Time=item.Recipe.Time

                });
            }
            if (newSearchResult.Any())
            {
                Tuple<bool, List<SummaryRecipeInfoResponse>> Results = new(true, result);
                return Ok(Results);
            }
            else
            {
                Tuple<bool, List<SummaryRecipeInfoResponse>> Results = new(false, result);
                return Ok(Results);
            }
        }
    }
}
