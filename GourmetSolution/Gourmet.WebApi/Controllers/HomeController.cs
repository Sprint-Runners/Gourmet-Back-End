using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;


namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IChefService _chefservice;
        private readonly IRecipeService _recipeservice;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly ICategoriesService _categoriesService;
        public HomeController(AppDbContext db, IChefService chefService, IImageProcessorService imageProcessorService, ICategoriesService categoriesService, IRecipeService recipeservice)
        {
            _db = db;
            _chefservice = chefService;
            _imageProcessorService = imageProcessorService;
            _categoriesService = categoriesService;
            _recipeservice = recipeservice;
        }
        [HttpGet("Home")]
        public async Task<IActionResult>  HomeAsync()
        {
            try
            {
                var random = new Random();
                var allIds = _db.Foods.Select(x => x.Id).ToList();
                var randomIds = allIds.OrderBy(x => random.Next()).Take(3).ToList();
                var randomRows = _db.Foods.Where(x => randomIds.Contains(x.Id)).ToList();
                List<FoodInformationResponse> randomFood = new List<FoodInformationResponse>();
                List<SummaryRecipeInfoResponse> Top_Recipes = new List<SummaryRecipeInfoResponse>();
                foreach (Food row in randomRows)
                {
                    row.ImgeUrl = await _imageProcessorService.GetImagebyFood(row.Name);
                    randomFood.Add(new FoodInformationResponse
                    {
                        Name = row.Name,
                        ImagePath = row.ImgeUrl,
                    }); ;
                }
                var chefs =  _db.Chefs.ToList();
                //var topChefs = chefs.OrderByDescending(async c => await _chefservice.GetChefScore(c.Id))
                //                    .Take(3)
                //                    .ToList();
                var topChefs = chefs.OrderByDescending(x=>x.Score)
                                    .Take(3)
                                    .ToList();
                List<TopChefResponse> TopChefs = new List<TopChefResponse>();
                foreach (Chef row in topChefs)
                {
                    row.ImageURL = await _imageProcessorService.GetImagebyUser(row.UserName);
                    //var chefrecipes = await _chefservice.GetRecipesByChefId(row.Id);

                    //var topchefrecipes = chefrecipes.OrderByDescending(x => x.Score)
                    //                .Take(5)
                    //                .ToList();
                    //List<SummaryRecipeInfoResponse> Top_Special_Chef_Recipes = new List<SummaryRecipeInfoResponse>();
                    //foreach (Recipe r in topchefrecipes)
                    //{
                    //    r.ImgeUrl = await _imageProcessorService.GetImagebyRecipe(r.food.Name, r.chef.UserName);
                    //    Top_Special_Chef_Recipes.Add(new SummaryRecipeInfoResponse
                    //    {
                    //        Name = r.food.Name,
                    //        ChefName = r.chef.FullName,
                    //        ImagePath = r.ImgeUrl,
                    //        Score = r.Score
                    //    });
                    //}
                    TopChefs.Add(new TopChefResponse
                    {
                        Name = row.FullName,
                        Score = row.Score,
                        AboutMe = row.Aboutme,
                        ImagePath = row.ImageURL,
                        //Top_Chef_Recipes = Top_Special_Chef_Recipes
                    });
                }
                var PSOIS = await _categoriesService.GetAllPSOICategory();
                var FTS = await _categoriesService.GetAllFTCategory();
                var MTS = await _categoriesService.GetAllMTCategory();
                List<CategoriesResponse> Categories = new List<CategoriesResponse>();
                foreach (var category in PSOIS)
                {
                    Categories.Add(new CategoriesResponse
                    {
                        Name = category.Name,
                        CategoryName = "Primary source of ingredient",
                        ImageUrl = await _imageProcessorService.GetImagebyCategory("PSOI", category.Name)

                    });
                }
                foreach (var category in FTS)
                {
                    Categories.Add(new CategoriesResponse
                    {
                        Name = category.Name,
                        CategoryName = "Food Type",
                        ImageUrl = await _imageProcessorService.GetImagebyCategory("FT", category.Name)

                    });
                }
                foreach (var category in MTS)
                {
                    Categories.Add(new CategoriesResponse
                    {
                        Name = category.Name,
                        CategoryName = "Meal Type",
                        ImageUrl = await _imageProcessorService.GetImagebyCategory("MT", category.Name)

                    });
                }
                var random_category = new Random();
                Categories = Categories.OrderBy(x => random_category.Next()).ToList();
                //var allrecipe = await _recipeservice.Get_All_Recipe();
                //var toprecipes = allrecipe.OrderByDescending(x => x.Score)
                //                    .Take(5)
                //                    .ToList();
                //foreach (Recipe row in toprecipes)
                //{
                //    row.ImgeUrl = await _imageProcessorService.GetImagebyRecipe(row.food.Name, row.chef.UserName);
                //    Top_Recipes.Add(new SummaryRecipeInfoResponse
                //    {
                //        Name = row.food.Name,
                //        ChefName = row.chef.FullName,
                //        ImagePath = row.ImgeUrl,
                //        Score = row.Score
                //    });
                //}
                //return Ok(TopChefs);
                Tuple<List<FoodInformationResponse>, List<TopChefResponse>, List<CategoriesResponse>> result = new Tuple<List<FoodInformationResponse>, List<TopChefResponse>, List<CategoriesResponse>>(randomFood, TopChefs, Categories);
                return Ok(result);
                return Ok((randomFood));
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        //public async Task<IEnumerable<>> GetAllCategories()
        //{

        //}
        //[HttpGet("TopChef")]
        //public async Task<IActionResult> GetTopChefFromDatabaseAsync()
        //{
        //    try
        //    {
        //        var chefs = await _db.Chefs.ToListAsync();
        //        var topChefs = chefs.OrderByDescending(async c => await _chefservice.GetChefScore(c.Id))
        //                            .Take(3)
        //                            .ToList();
        //        List<TopChefResponse> TopChefs = new List<TopChefResponse>();
        //        foreach (Chef row in topChefs)
        //        {
        //            row.ImageURL = _imageProcessorService.GetImagebyUser(row.UserName);
        //            TopChefs.Add(new TopChefResponse
        //            {
        //                Name = row.UserName,
        //                Score = row.Score,
        //                ImagePath = row.ImageURL
        //            }); ;
        //        }
        //        return Ok(TopChefs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(detail: ex.Message, statusCode: 400);
        //    }
        //}

    }
}


