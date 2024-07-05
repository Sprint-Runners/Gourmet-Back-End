using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Relations;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Linq;


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
        private readonly UserManager<Chef> _userManager;
        public HomeController(AppDbContext db, IChefService chefService, IImageProcessorService imageProcessorService, ICategoriesService categoriesService, IRecipeService recipeservice,UserManager<Chef>userManager)
        {
            _db = db;
            _chefservice = chefService;
            _imageProcessorService = imageProcessorService;
            _categoriesService = categoriesService;
            _recipeservice = recipeservice;
            _userManager = userManager;
        }
        [HttpGet("Home")]
        public async Task<IActionResult>  HomeAsync()
        {
            try
            {
                var random = new Random();
                var allIds = _db.Foods.Select(x => x.Id).ToList();
                var randomIds = allIds.OrderBy(x => random.Next()).Take(4).ToList();
                var randomRows = _db.Foods.Where(x => randomIds.Contains(x.Id)).ToList();
                List<FoodInformationResponse> randomFood = new List<FoodInformationResponse>();
                //List<SummaryRecipeInfoResponse> Top_Recipes = new List<SummaryRecipeInfoResponse>();
                foreach (Food row in randomRows)
                {
                    row.ImgeUrl = await _imageProcessorService.GetImagebyFood(row.Name);
                    randomFood.Add(new FoodInformationResponse
                    {
                        Name = row.Name.Replace('_',' '),
                        ImagePath = row.ImgeUrl,
                    }); ;
                }
                var chefs =  _db.Chefs.ToList();
                foreach(var item in chefs)
                {
                    item.Score=await _chefservice.GetChefScore(item.Id);
                }
                _db.SaveChanges();
                var topChefs = chefs.OrderByDescending(c => c.Score)
                                    .Take(5)
                                    .ToList();
                //var topChefs = chefs.OrderByDescending(x=>x.Score)
                //                    .Take(3)
                //                    .ToList();
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
                        UserName=row.UserName,
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
                var menucategory= Categories.OrderBy(x => random_category.Next()).Take(6).ToList();
                var AllRecipes = _db.Recipes.OrderBy(r=>r.CreatTime).ToList();
                AllRecipes = AllRecipes.Where(r => r.IsAccepted == true && r.IsReject == false && r.FoodString == "" && r.NotExistIngredients == "").ToList();
                List<SummaryCategoryRecipeResponse> categoryRecipeResponses = new List<SummaryCategoryRecipeResponse>();
                for (int i=0;i<6; i++)
                {
                    if (menucategory[i].CategoryName == "Meal Type") {
                        var isExitsMT = _db.MTs.Where(x => x.Name == menucategory[i].Name).FirstOrDefault();
                        var menurecipe= AllRecipes.Where(r => r.Meal_TypeId == isExitsMT.Id).Count() > 0 ? AllRecipes.Where(r => r.Meal_TypeId == isExitsMT.Id).First():AllRecipes.First();
                        var Recipeingredients = _db.RecipeIngredients.Where(x => x.RecipeId == menurecipe.Id).ToList();
                        string ings = "";
                        var lasting = Recipeingredients.Last();
                        foreach (var j in Recipeingredients)
                        {
                            if (j == lasting)
                                break;
                            var isExitsIngredient = _db.Ingredients.Where(x => x.Id == j.IngredientId).FirstOrDefault();
                            ings += isExitsIngredient.Name + ',';
                        }
                        var isExitsIngredient2 = _db.Ingredients.Where(x => x.Id == lasting.IngredientId).FirstOrDefault();
                        ings += isExitsIngredient2.Name;
                        categoryRecipeResponses.Add(new SummaryCategoryRecipeResponse
                        {
                            Ingredients = ings,
                            Description = menurecipe.Description,
                            Category = isExitsMT.Name,
                            Name = menurecipe.Name
                        });
                    }
                    if (menucategory[i].CategoryName == "Primary source of ingredient")
                    {
                        var isExitsPSOI = _db.PSOIs.Where(x => x.Name == menucategory[i].Name).FirstOrDefault();
                        var menurecipe = AllRecipes.Where(r => r.Primary_Source_of_IngredientId == isExitsPSOI.Id).Count() > 0 ? AllRecipes.Where(r => r.Primary_Source_of_IngredientId== isExitsPSOI.Id).First():AllRecipes.First();
                        var Recipeingredients = _db.RecipeIngredients.Where(x => x.RecipeId == menurecipe.Id).ToList();
                        string ings = "";
                        var lasting = Recipeingredients.Last();
                        foreach (var j in Recipeingredients)
                        {
                            if (j == lasting)
                                break;
                            var isExitsIngredient = _db.Ingredients.Where(x => x.Id == j.IngredientId).FirstOrDefault();
                            ings += isExitsIngredient.Name + ',';
                        }
                        var isExitsIngredient2 = _db.Ingredients.Where(x => x.Id == lasting.IngredientId).FirstOrDefault();
                        ings += isExitsIngredient2.Name;
                        categoryRecipeResponses.Add(new SummaryCategoryRecipeResponse
                        {
                            Ingredients = ings,
                            Description = menurecipe.Description,
                            Category = isExitsPSOI.Name,
                            Name = menurecipe.Name
                        });

                    }
                    if (menucategory[i].CategoryName == "Food Type")
                    {
                        var isExitsFT = _db.FTs.Where(x => x.Name == menucategory[i].Name).FirstOrDefault();
                        var menurecipe = AllRecipes.Where(r => r.Food_typeId == isExitsFT.Id).Count()>0? AllRecipes.Where(r => r.Food_typeId == isExitsFT.Id).First():AllRecipes.First();
                        var Recipeingredients = _db.RecipeIngredients.Where(x => x.RecipeId == menurecipe.Id).ToList();
                        string ings = "";
                        var lasting = Recipeingredients.Last();
                        foreach (var j in Recipeingredients)
                        {
                            if (j == lasting)
                                break;
                            var isExitsIngredient = _db.Ingredients.Where(x => x.Id == j.IngredientId).FirstOrDefault();
                            ings += isExitsIngredient.Name + ',';
                        }
                        var isExitsIngredient2 = _db.Ingredients.Where(x => x.Id == lasting.IngredientId).FirstOrDefault();
                        ings += isExitsIngredient2.Name;
                        categoryRecipeResponses.Add(new SummaryCategoryRecipeResponse
                        {
                            Ingredients = ings,
                            Description = menurecipe.Description,
                            Category = isExitsFT.Name,
                            Name = menurecipe.Name
                        });
                    }
                }  
                
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
                //var random_category_recipe = new Random();
                //var AllRecipes = _db.Recipes.ToList();
                //AllRecipes = AllRecipes.Where(r => r.IsAccepted == true && r.IsReject == false && r.FoodString == "" && r.NotExistIngredients == "").ToList();
                //var quicks = AllRecipes.Where(r => r.Time <= 30).OrderBy(x => random_category_recipe.Next()).First();
                //Meal_Type breakfast = _db.MTs.Where(r => r.Name == "Breakfast").FirstOrDefault();
                //var breakfasts = AllRecipes.Where(r => r.Meal_TypeId == breakfast.Id).OrderBy(x => random_category_recipe.Next()).First(); ;
                //Primary_Source_of_Ingredient vegan = _db.PSOIs.Where(r => r.Name == "Vegan").FirstOrDefault();
                //var vegans = AllRecipes.Where(r => r.Primary_Source_of_IngredientId == vegan.Id).OrderBy(x => random_category_recipe.Next()).First(); ;
                //Meal_Type dinner = _db.MTs.Where(r => r.Name == "Dinner").FirstOrDefault();
                //Meal_Type lunch = _db.MTs.Where(r => r.Name == "Lunch").FirstOrDefault();
                //var mains = AllRecipes.Where(r => r.Meal_TypeId == dinner.Id || r.Meal_TypeId == lunch.Id).OrderBy(x => random_category_recipe.Next()).First(); ;
                //Food_type fast_food = _db.FTs.Where(r => r.Name == "FastFood").FirstOrDefault();
                //var fastfoods = AllRecipes.Where(r => r.Food_typeId == fast_food.Id ).OrderBy(x => random_category_recipe.Next()).First(); ;
                //Food_type salad = _db.FTs.Where(r => r.Name == "Salad").FirstOrDefault();
                //var salads = AllRecipes.Where(r => r.Food_typeId == salad.Id).OrderBy(x => random_category_recipe.Next()).First(); ;

                //List<SummaryCategoryRecipeResponse>categoryRecipeResponses = new List<SummaryCategoryRecipeResponse>();
                //var Recipeingredients = _db.RecipeIngredients.Where(x => x.RecipeId == quicks.Id).ToList();
                //string ings = "";
                //var lasting = Recipeingredients.Last();
                //foreach (var i in Recipeingredients)
                //{
                //    if (i == lasting)
                //        break;
                //    var isExitsIngredient = _db.Ingredients.Where(x => x.Id == i.IngredientId).FirstOrDefault();
                //    ings+=isExitsIngredient.Name+',';
                //}
                //var isExitsIngredient2 = _db.Ingredients.Where(x => x.Id == lasting.IngredientId).FirstOrDefault();
                //ings += isExitsIngredient2.Name;
                //categoryRecipeResponses.Add(new SummaryCategoryRecipeResponse
                //{
                //    Ingredients = ings,
                //    Description = quicks.Description,
                //    Category = "Quick",
                //    Name = quicks.Name
                //});
                //Recipeingredients = _db.RecipeIngredients.Where(x => x.RecipeId == quicks.Id).ToList();
                //ings = "";
                //lasting = Recipeingredients.Last();
                //foreach (var i in Recipeingredients)
                //{
                //    if (i == lasting)
                //        break;
                //    var isExitsIngredient = _db.Ingredients.Where(x => x.Id == i.IngredientId).FirstOrDefault();
                //    ings += isExitsIngredient.Name + ',';
                //}
                //isExitsIngredient2 = _db.Ingredients.Where(x => x.Id == lasting.IngredientId).FirstOrDefault();
                //ings += isExitsIngredient2.Name;
                //categoryRecipeResponses.Add(new SummaryCategoryRecipeResponse
                //{
                //    Ingredients = ings,
                //    Description = quicks.Description,
                //    Category = "Quick",
                //    Name = quicks.Name
                //});
                Tuple<List<FoodInformationResponse>, List<TopChefResponse>, List<CategoriesResponse>,List<SummaryCategoryRecipeResponse>> result = new Tuple<List<FoodInformationResponse>, List<TopChefResponse>, List<CategoriesResponse>,List<SummaryCategoryRecipeResponse>>(randomFood, TopChefs, Categories, categoryRecipeResponses);
                return Ok(result);
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


