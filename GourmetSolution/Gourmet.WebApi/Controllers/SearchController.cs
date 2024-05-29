using FuzzySharp;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

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
        private readonly ICategoriesService _categoriesService;
        public SearchController(AppDbContext db, IRecipeService recipeService, UserManager<Chef> userManager, IImageProcessorService imageProcessorService, ICategoriesService categoriesService)
        {
            _db = db;
            _recipeServicel = recipeService;
            _userManager = userManager;
            _imageProcessorService = imageProcessorService;
            _categoriesService = categoriesService;
        }
        [HttpPut("Search_Ingredient")]
        public async Task<IActionResult> Search_Ingredient(SearchRequest request)
        {
            string searchTerm = request.SearchTerm.ToLower().Trim();
            var allIngredients = _db.Ingredients.ToList();

            var searchResults = allIngredients.Select(obj => new SearchResponse
            {
                SearchName = obj.Name,
                PartialRatioScore = Fuzz.PartialRatio(obj.Name.ToLower().Trim(), searchTerm),
                RatioScore = Fuzz.Ratio(obj.Name.ToLower().Trim(), searchTerm)
            }).Where(result => result.PartialRatioScore >= 50 || result.RatioScore >= 70)
            .OrderByDescending(result => Math.Max(result.PartialRatioScore, result.RatioScore))
            .ToList();
            var Ingredient = allIngredients.Where(ing => ing.Name.ToLower() == searchTerm.ToLower()).FirstOrDefault();
            bool BoolResult;
            if (Ingredient == null)
            {
                BoolResult = false;
            }
            else
            {
                BoolResult = true;
            }
            if (searchResults.Any())
            {
                Tuple<bool, List<SearchResponse>, string> Results = new(BoolResult, searchResults, "succes");
                return Ok(Results);
            }
            else
            {
                Tuple<bool, List<SearchResponse>, string> Results = new(BoolResult, searchResults, "Not Found");
                return Ok(Results);
            }
        }
        [HttpPut("Search_Food")]
        public async Task<IActionResult> Search_Food(SearchRequest request)
        {
            string searchTerm = request.SearchTerm.ToLower().Trim();
            var allFoods =  _db.Foods.ToList();

            var searchResults = allFoods.Select(obj => new SearchResponse
            {
                SearchName = obj.Name,
                PartialRatioScore = Fuzz.PartialRatio(obj.Name.ToLower().Trim(), searchTerm),
                RatioScore = Fuzz.Ratio(obj.Name.ToLower().Trim(), searchTerm)
            }).Where(result => result.PartialRatioScore >= 50 || result.RatioScore >= 50)
            .OrderByDescending(result => Math.Max(result.PartialRatioScore, result.RatioScore))
            .ToList();
            var Food = allFoods.Where(fo => fo.Name.ToLower() == searchTerm.ToLower()).FirstOrDefault();
            bool BoolResult;
            if (Food == null)
            {
                BoolResult = false;
            }
            else
            {
                BoolResult = true;
            }
            if (searchResults.Any())
            {
                Tuple<bool, List<SearchResponse>, string> Results = new(BoolResult, searchResults, "Succes");
                return Ok(Results);
            }
            else
            {
                Tuple<bool, List<SearchResponse>, string> Results = new(BoolResult, searchResults, "Not Found");
                return Ok(Results);
            }
        }
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var PSOIS = await _categoriesService.GetAllPSOICategory();
            var FTS = await _categoriesService.GetAllFTCategory();
            var MTS = await _categoriesService.GetAllMTCategory();
            var NS = await _categoriesService.GetAllNCategory();
            var DLS = await _categoriesService.GetAllDLCategory();
            var CMS = await _categoriesService.GetAllCMCategory();
            Dictionary<string, List<CategoriesResponse>> Categories = new Dictionary<string, List<CategoriesResponse>>();
            List<CategoriesResponse> PSOIsResopnse = new List<CategoriesResponse>();
            foreach (var category in PSOIS)
            {
                PSOIsResopnse.Add(new CategoriesResponse
                {
                    Name = category.Name,
                    CategoryName = "Primary source of ingredient",
                    ImageUrl = await _imageProcessorService.GetImagebyCategory("PSOI", category.Name)

                });
            }
            Categories.Add("Primary source of ingredient", PSOIsResopnse);
            List<CategoriesResponse> FTsResopnse = new List<CategoriesResponse>();
            foreach (var category in FTS)
            {
                FTsResopnse.Add(new CategoriesResponse
                {
                    Name = category.Name,
                    CategoryName = "Food Type",
                    ImageUrl = await _imageProcessorService.GetImagebyCategory("FT", category.Name)

                });
            }
            Categories.Add("Food Type", FTsResopnse);
            List<CategoriesResponse> CMsResopnse = new List<CategoriesResponse>();
            foreach (var category in CMS)
            {
                CMsResopnse.Add(new CategoriesResponse
                {
                    Name = category.Name,
                    CategoryName = "Cooking Method",
                    ImageUrl = await _imageProcessorService.GetImagebyCategory("CM", category.Name)

                });
            }
            Categories.Add("Cooking Method", CMsResopnse);
            List<CategoriesResponse> NsResopnse = new List<CategoriesResponse>();
            foreach (var category in NS)
            {
                NsResopnse.Add(new CategoriesResponse
                {
                    Name = category.Name,
                    CategoryName = "Nationality",
                    ImageUrl = await _imageProcessorService.GetImagebyCategory("N", category.Name)

                });
            }
            Categories.Add("Nationality", NsResopnse);
            List<CategoriesResponse> MTsResopnse = new List<CategoriesResponse>();
            foreach (var category in MTS)
            {
                MTsResopnse.Add(new CategoriesResponse
                {
                    Name = category.Name,
                    CategoryName = "Meal Type",
                    ImageUrl = await _imageProcessorService.GetImagebyCategory("MT", category.Name)

                });
            }
            Categories.Add("Meal Type", MTsResopnse);
            List<CategoriesResponse> DLsResopnse = new List<CategoriesResponse>();
            foreach (var category in DLS)
            {
                DLsResopnse.Add(new CategoriesResponse
                {
                    Name = category.Name,
                    CategoryName = "Difficulty_Level",
                    ImageUrl = await _imageProcessorService.GetImagebyCategory("DL", category.Name)

                });
            }
            Categories.Add("Difficulty_Level", DLsResopnse);
            return Ok(Categories);


        }

        [HttpPut("FullSearch")]
        public async Task<IActionResult> FullSearch(FullSearchRequest request)
        {
            var searchResults = new List<SearchRecipe>();
            List<SummaryRecipeInfoResponse> result = new List<SummaryRecipeInfoResponse>();
            
            if (request.Text != "")
            {
                request.Text = request.Text.ToLower().Trim();
                var AllRecipes = _db.Recipes.ToList();
                AllRecipes = AllRecipes.Where(r => r.IsAccepted == true && r.IsReject == false && r.FoodString=="" && r.NotExistIngredients=="").ToList();
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
                AllRecipes = AllRecipes.Where(r => r.IsAccepted == true && r.IsReject == false).ToList();
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
            var newsearchResults1 = new List<SearchRecipe>();
            if (request.FoodName != "")
            {
                foreach (var item in searchResults)
                {
                    var isExitsFood1 = _db.Foods.Where(x => x.Id == item.Recipe.FoodId).FirstOrDefault();
                    if (isExitsFood1.Name == request.FoodName)
                    {
                        newsearchResults1.Add(item);
                    }
                }
            }
            else
            {
                newsearchResults1 = searchResults;
            }
            var newSearchResult2 = new List<SearchRecipe>();
            if (request.ingredients.Count > 0)
            {
                foreach (var item in newsearchResults1)
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
                    if ((count >= 3 && request.ingredients.Count == 4) || (count >= 2 && request.ingredients.Count == 3) || (count == 2 && request.ingredients.Count == 2) || (count == 1 && request.ingredients.Count == 1))
                    {
                        newSearchResult2.Add(item);
                    }
                }
            }
            else
            {
                newSearchResult2 = newsearchResults1;
            }
            foreach (var item in newSearchResult2)
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
                    Time = item.Recipe.Time

                });
            }
            if (newSearchResult2.Any())
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
