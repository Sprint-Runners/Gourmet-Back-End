using FuzzySharp;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
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
    public class RecipeController : ControllerBase
    {
        private readonly AppDbContext _db;
        //bayad dorost she inteface nadare
        private readonly RecipeService _recipeService;
        private readonly UserManager<Chef> _userManager;
        private readonly IJwt _jwtService;
        public RecipeController(AppDbContext db,RecipeService recipeService,UserManager<Chef>userManager,IJwt jwt)
        {
            _db = db;
            _recipeService = recipeService;
            _userManager = userManager;
            _jwtService = jwt;
        }
        [HttpGet("Search_Ingredient")]
        public async Task<IActionResult> Search_Ingredient(string searchTerm)
        {
            searchTerm = searchTerm.ToLower().Trim();
            var allIngredients = await _db.Ingredients.ToListAsync();

            var searchResults = allIngredients.Select(obj => new
            {
                Ingredient_Name = obj.Name,
                PartialRatioScore = Fuzz.PartialRatio(obj.Name.ToLower().Trim(), searchTerm),
                RatioScore = Fuzz.Ratio(obj.Name.ToLower().Trim(), searchTerm)
            }).Where(result => result.PartialRatioScore >= 50 || result.RatioScore >= 50)
            .OrderByDescending(result => Math.Max(result.PartialRatioScore, result.RatioScore))
            .ToList();

            if (searchResults.Any())
            {
                return Ok(searchResults);
            }
            else
            {
                return Problem(detail: "Not Found", statusCode: 400);
            }
        }
        [HttpGet("Search_Food")]
        public async Task<IActionResult> Search_Food(string searchTerm)
        {
            searchTerm = searchTerm.ToLower().Trim();
            var allFoods = await _db.Foods.ToListAsync();

            var searchResults = allFoods.Select(obj => new
            {
                Ingredient_Name = obj.Name,
                PartialRatioScore = Fuzz.PartialRatio(obj.Name.ToLower().Trim(), searchTerm),
                RatioScore = Fuzz.Ratio(obj.Name.ToLower().Trim(), searchTerm)
            }).Where(result => result.PartialRatioScore >= 50 || result.RatioScore >= 50)
            .OrderByDescending(result => Math.Max(result.PartialRatioScore, result.RatioScore))
            .ToList();

            if (searchResults.Any())
            {
                return Ok(searchResults);
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
            string token = HttpContext.Request.Headers["Authorization"];
            string username = _jwtService.DecodeToken(token);
            var isExistsUser = await _userManager.FindByNameAsync(username);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            if (request.NotExistFoodName!=null || request.Not_Exist_List_Ingriedents != null)
            {
                var result = await _recipeService.CreateInCompleteRecipe(request,isExistsUser.Id,username);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            var result1 = await _recipeService.CreateRecipeByChef(request, isExistsUser.Id, username);
            if (result1.IsSucceed)
            {
                return Ok(result1);
            }
            return Problem(detail: result1.Message, statusCode: 400);

        }
    }
}
