using FuzzySharp;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddRecipeController : ControllerBase
    {
        private readonly AppDbContext _db;
        //bayad dorost she inteface nadare
        private readonly IRecipeService _recipeService;
        private readonly UserManager<Chef> _userManager;
        private readonly IJwt _jwtService;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly ICategoriesService _categoriesService;
        public AddRecipeController(AppDbContext db, IRecipeService recipeService, UserManager<Chef> userManager, IJwt jwt, IImageProcessorService imageProcessorService, ICategoriesService categoriesService)
        {
            _db = db;
            _recipeService = recipeService;
            _userManager = userManager;
            _jwtService = jwt;
            _imageProcessorService = imageProcessorService;
            _categoriesService = categoriesService;
        }
        [HttpGet("GetAllCategory")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> GetAllCategory()
        {
            var PSOIS = await _categoriesService.GetAllPSOICategory();
            var FTS = await _categoriesService.GetAllFTCategory();
            var MTS = await _categoriesService.GetAllMTCategory();
            var NS = await _categoriesService.GetAllNCategory();
            var DLS = await _categoriesService.GetAllDLCategory();
            var CMS = await _categoriesService.GetAllCMCategory();
            Dictionary<string,List<CategoriesResponse>> Categories =new Dictionary<string, List<CategoriesResponse>>();
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
        [HttpGet("Validate_Recipe_Name")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> ValidateRecipeName(string searchTerm)
        {
            searchTerm = searchTerm.ToLower().Trim();
            if (searchTerm.Length < 5)
            {
                return Problem(detail: "This name is very short", statusCode: 400);
            }
            var allRecipes = await _db.Recipes.ToListAsync();
            var Recipe=_db.Recipes.Where(r => r.Name.ToLower() == searchTerm.ToLower()).FirstOrDefault();
            if (Recipe == null)
            {
                return Ok();
            }
            else
            {
                return Problem(detail: "This name is used", statusCode: 400);
            }
        }
        [HttpGet("Search_Ingredient")]
        [Authorize(Roles =StaticUserRoles.CHEF)]
        public async Task<IActionResult> Search_Ingredient(string searchTerm)
        {
            searchTerm = searchTerm.ToLower().Trim();
            var allIngredients = await _db.Ingredients.ToListAsync();

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
                Tuple<bool, List<SearchResponse>> Results = new (BoolResult, searchResults);
                return Ok(Results);
            }
            else
            {
                return Problem(detail: "Not Found", statusCode: 400);
            }
        }
        [HttpGet("Search_Food")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Search_Food(string searchTerm)
        {
            searchTerm = searchTerm.ToLower().Trim();
            var allFoods = await _db.Foods.ToListAsync();

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
                Tuple<bool, List<SearchResponse>> Results = new(BoolResult, searchResults);
                return Ok(Results);
            }
            else
            {
                return Problem(detail: "Not Found", statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_Recipe")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Add_Recipe(AddRecipeRequest request)
        {
            try
            {
                //string token = HttpContext.Request.Headers["Authorization"];
                //string username = _jwtService.DecodeToken(token);
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var finduser = await _userManager.GetUserAsync(currentUser);
                var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
                if (isExistsUser != null)
                    return Problem(detail: "UserName not Exists", statusCode: 400);
                request.NumberOfPicture = Request.Form.Files.Count;
                if(request.NumberOfPicture > 5) 
                {
                    return Problem(detail: "The number of photos is more than the limit", statusCode: 400);
                }
                if (request.NotExistFoodName != null || request.List_Ingriedents.Where(ING=>ING.Item4==false).ToList().Count != 0)
                {
                    var result = await _recipeService.CreateInCompleteRecipe(request, isExistsUser.Id, finduser.UserName);
                    if (result.IsSucceed)
                    {
                        for (int i = 0; i < request.NumberOfPicture; i++)
                        {
                            var file = Request.Form.Files[i];
                            var ResultImage = await _imageProcessorService.UploadRecipeImage(file, result.recipe.FoodString, result.recipe.chef.UserName, result.recipe.Name,i);
                            if (!ResultImage.IsSucceed)
                            {
                                //result.recipe. = await _imageProcessorService.GetImagebyRecipe(result.recipe.FoodString, result.recipe.chef.UserName, result.recipe.Name,i);
                                //return Ok(result.recipe);

                                return Problem(detail: ResultImage.Message, statusCode: 400);
                            }
                        }
                        return Ok(result.Message);
                    }
                    return Problem(detail: result.Message, statusCode: 400);
                }
                var result1 = await _recipeService.CreateRecipeByChef(request, isExistsUser.Id, finduser.UserName);
                if (result1.IsSucceed)
                {
                    for (int i = 0; i < request.NumberOfPicture; i++)
                    {
                        var file = Request.Form.Files[i];
                        var ResultImage = await _imageProcessorService.UploadRecipeImage(file, result1.recipe.food.Name, result1.recipe.chef.UserName, result1.recipe.Name, i);
                        if (!ResultImage.IsSucceed)
                        {
                            //result.recipe. = await _imageProcessorService.GetImagebyRecipe(result.recipe.FoodString, result.recipe.chef.UserName, result.recipe.Name,i);
                            //return Ok(result.recipe);

                            return Problem(detail: ResultImage.Message, statusCode: 400);
                        }
                    }
                    return Ok(result1.Message);

                }
                return Problem(detail: result1.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
    }
}
