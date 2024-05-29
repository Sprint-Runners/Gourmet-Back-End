using FuzzySharp;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.Relations;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

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
        [HttpPut("Validate_Recipe_Name")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> ValidateRecipeName(SearchRequest request)
        {
            string searchTerm = request.SearchTerm.ToLower().Trim();
            if (searchTerm.Length < 5)
            {
                return Ok(new SearchRecipeResponse
                {
                    Success = false,
                    Message = "This name is very short"
                });
            }
            var allRecipes = await _db.Recipes.ToListAsync();
            var Recipe = _db.Recipes.Where(r => r.Name.ToLower() == searchTerm.ToLower()).FirstOrDefault();
            if (Recipe == null)
            {
                return Ok(new SearchRecipeResponse
                {
                    Success = true,
                    Message = "this name is available"
                });

            }
            else
            {
                return Ok(new SearchRecipeResponse
                {
                    Success = false,
                    Message = "This name is used"
                });

            }
        }
        [HttpPut("Search_Ingredient")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Search_Ingredient(SearchRequest request)
        {
            string searchTerm = request.SearchTerm.ToLower().Trim();
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
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Search_Food(SearchRequest request)
        {
            string searchTerm = request.SearchTerm.ToLower().Trim();
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
                Tuple<bool, List<SearchResponse>, string> Results = new(BoolResult, searchResults, "Succes");
                return Ok(Results);
            }
            else
            {
                Tuple<bool, List<SearchResponse>, string> Results = new(BoolResult, searchResults, "Not Found");
                return Ok(Results);
            }
        }
        [HttpPost]
        [Route("Add_Recipe")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Add_Recipe(AddRecipeRequest request)
        {

            //string token = HttpContext.Request.Headers["Authorization"];
            //string username = _jwtService.DecodeToken(token);
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser == null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            //request.NumberOfPicture = Request.Form.Files.Count;
            //if(request.NumberOfPicture > 5) 
            //{
            //    return Problem(detail: "The number of photos is more than the limit", statusCode: 400);
            //}
            if (request.NotExistFoodName != "" || request.List_Ingriedents.Where(ING => ING.Item4 == false).ToList().Count != 0)
            {
                Console.WriteLine("im in incomplete recipe8**************");
                var result = await _recipeService.CreateInCompleteRecipe(request, isExistsUser.Id, finduser.UserName);
                if (result.IsSucceed)
                {
                    //for (int i = 0; i < request.NumberOfPicture; i++)
                    //{
                    //    var file = Request.Form.Files[i];
                    //    var ResultImage = await _imageProcessorService.UploadRecipeImage(file, result.recipe.FoodString, result.recipe.chef.UserName, result.recipe.Name,i);
                    //    if (!ResultImage.IsSucceed)
                    //    {
                    //        //result.recipe. = await _imageProcessorService.GetImagebyRecipe(result.recipe.FoodString, result.recipe.chef.UserName, result.recipe.Name,i);
                    //        //return Ok(result.recipe);

                    //        return Problem(detail: ResultImage.Message, statusCode: 400);
                    //    }
                    //}
                    return Ok(result.Message);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            Console.WriteLine("im in ***********8recipe8**************");
            var result1 = await _recipeService.CreateRecipeByChef(request, isExistsUser.Id, finduser.UserName);
            if (result1.IsSucceed)
            {
                //for (int i = 0; i < request.NumberOfPicture; i++)
                //{
                //    var file = Request.Form.Files[i];
                //    var ResultImage = await _imageProcessorService.UploadRecipeImage(file, result1.recipe.food.Name, result1.recipe.chef.UserName, result1.recipe.Name, i);
                //    if (!ResultImage.IsSucceed)
                //    {
                //        //result.recipe. = await _imageProcessorService.GetImagebyRecipe(result.recipe.FoodString, result.recipe.chef.UserName, result.recipe.Name,i);
                //        //return Ok(result.recipe);

                //        return Problem(detail: ResultImage.Message, statusCode: 400);
                //    }
                //}
                return Ok(result1.Message);

            }
            return Problem(detail: result1.Message, statusCode: 400);
        }
        [HttpPost]
        [Route("Add_Image_Recipe")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Add_Image_Recipe()
        {
            var isExistRecipe=new Recipe();
            try
            {
                //string token = HttpContext.Request.Headers["Authorization"];
                //string username = _jwtService.DecodeToken(token);
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var finduser = await _userManager.GetUserAsync(currentUser);
                var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
                if (isExistsUser == null)
                    return Problem(detail: "UserName not Exists", statusCode: 400);
                ImageRecipeRequest request = new ImageRecipeRequest
                {
                    RecipeName = Request.Form["RecipeName"],
                    ChefName = finduser.UserName,
                    FoodName = Request.Form["FoodName"]
                };
                isExistRecipe = await _recipeService.Search_Recipe(request.FoodName, request.ChefName, request.RecipeName);
                int NumberOfPicture = Request.Form.Files.Count;
                if (NumberOfPicture > 5)
                {
                    _db.Recipes.Remove(isExistRecipe);
                    _db.SaveChanges();
                    return Problem(detail: "The number of photos is more than the limit", statusCode: 400);
                }
                for (int i = 0; i < NumberOfPicture; i++)
                {
                    var file = Request.Form.Files[i];
                    var ResultImage = await _imageProcessorService.UploadRecipeImage(file, request.FoodName, request.ChefName, request.RecipeName, i+1);
                    if (!ResultImage.IsSucceed)
                    {
                        _db.Recipes.Remove(isExistRecipe);
                        _db.SaveChanges();
                        return Problem(detail: ResultImage.Message, statusCode: 400);
                    }
                }
                return Ok(new GeneralResponse { Message = "upload image succes" });
            }
            catch (Exception ex)
            {
                _db.Recipes.Remove(isExistRecipe);
                _db.SaveChanges();
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPut]
        [Route("Show_Recipe_Info")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Show_Recipe_Info(ShowRecipeForAdminOrChefRequest request )
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            var item = _db.Recipes.Where(x => x.Id == request.ID).FirstOrDefault();
            var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
            var allPSOI = _db.PSOIs.ToList();
            var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
            var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
            var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
            var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
            var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
            var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
            string Foodname = "";
            if (isExitsFood == null)
            {
                Foodname = item.FoodString;

            }
            else
            {
                Foodname = isExitsFood.Name;
                item.FoodString = "";
                item.FoodId = isExitsFood.Id;
                _db.Recipes.Update(item);
                _db.SaveChanges();
            }
            var ImageUrlRecipe1 = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 1);
            var ImageUrlRecipe2 = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 2);
            var ImageUrlRecipe3 = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 3);
            var ImageUrlRecipe4 = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 4);
            var ImageUrlRecipe5 = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 5);

            var steps = _db.RecipeSteps.Where(x => x.RecipeId == item.Id).OrderBy(x=>x.Number).Select(x=>new Tuple<string,string>(x.Number.ToString(),x.explenation)).ToList();
            var notExistingredient = item.NotExistIngredients.Split('.');
            List<Tuple<string, string, string, bool>> List_Ingriedents = new List<Tuple<string, string, string, bool>>(); 
            foreach(var ing in notExistingredient)
            {
                if (ing != "")
                {
                    var ingr = ing.Split(',');
                    var isExitsIngredient = _db.Ingredients.Where(x => x.Name == ingr[0]).FirstOrDefault();
                    if ( isExitsIngredient!= null)
                    {
                        List_Ingriedents.Add(new Tuple<string, string, string, bool>(ingr[0], ingr[1], ingr[2],true));
                        RecipeIngredients row = new RecipeIngredients
                        {
                            RecipeId = item.Id,
                            IngredientId = isExitsIngredient.Id,
                            Quantity = double.Parse(ingr[1]),
                            Unit = ingr[2]
                        };
                        _db.RecipeIngredients.Add(row);
                        _db.SaveChanges();
                    }
                    else
                    {
                        List_Ingriedents.Add(new Tuple<string, string, string, bool>(ingr[0], ingr[1], ingr[2], false));
                    }
                    
                }
            }
            var Recipeingredients=_db.RecipeIngredients.Where(x => x.RecipeId == item.Id).ToList();
            foreach(var i in Recipeingredients)
            {
                var isExitsIngredient = _db.Ingredients.Where(x => x.Id == i.IngredientId).FirstOrDefault();
                List_Ingriedents.Add(new Tuple<string, string, string, bool>(isExitsIngredient.Name, i.Quantity.ToString(), i.Unit, true));
            }
            var NotExistingredient = List_Ingriedents.Where(x => x.Item4 == false).ToList();
            var Existingredient = List_Ingriedents.Where(x => x.Item4 == true).ToList();
            var NotExistIngredients ="";
            foreach (var i in NotExistingredient)
            {
                string ingredient = i.Item1 + "," + i.Item2 + "," + i.Item3;
                NotExistIngredients = NotExistIngredients + ingredient + ".";
            }
            item.NotExistIngredients = NotExistIngredients;
            if (item.FoodString=="" && item.NotExistIngredients.Count() != 0)
            {
                item.IsCompelete = true;
            }
            _db.Recipes.Update(item);
            _db.SaveChanges();
            return Ok(new ShowRecipeForAdminOrChefResponse
            {
                ID = item.Id,
                ImageURL1 = ImageUrlRecipe1,
                ImageURL2 = ImageUrlRecipe2,
                ImageURL3 = ImageUrlRecipe3,
                ImageURL4 = ImageUrlRecipe4,
                ImageURL5 = ImageUrlRecipe5,
                Name = item.Name,
                FoodName = Foodname,
                NotExistFoodName=item.FoodString,
                cooking_method= isExitsCM.Name,
                difficulty_level = isExistDL.Name,
                Description = item.Description,
                food_type = isExitsFT.Name,
                meal_type = isExitsMT.Name,
                nationality = isExitsN.Name,
                Time = item.Time.ToString(),
                primary_source_of_ingredient= isExitsPSOI.Name,
                List_Ingriedents=List_Ingriedents,
                Steps=steps,
            });
        }
        [HttpPost]
        [Route("Edit_Recipe")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Edit_Recipe(EditRecipeRequest request)
        {

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser == null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var item = _db.Recipes.Where(x => x.Id == request.ID).FirstOrDefault();
            Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH^^^^^^^^^^^^^^^^^^^1111111111111111111111111111^^^^^^^^",request.ID);
            var isExitsFood = _db.Foods.Where(x => item.FoodId!=null && x.Id == item.FoodId).FirstOrDefault();
            Console.WriteLine("HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            string Foodname = "";
            if (isExitsFood == null)
                Foodname = item.FoodString;
            else
            {
                Foodname = isExitsFood.Name;
            }
            for (int i = 0; i < 5; i++)
            {
                _imageProcessorService.RemoveRecipeImage(Foodname, isExistsUser.UserName, item.Name, i);
            }
            var Result = await _recipeService.DeleteRecipe(Foodname, isExistsUser.UserName, item.Name, isExistsUser.Id);
            if (!Result.IsSucceed)
            {
                return Ok(new GenerallResponse { Success = false, Message = "Edit was not successful" });
            }
            AddRecipeRequest new_request = new AddRecipeRequest
            {
                Name = request.Name,
                cooking_method = request.cooking_method,
                food_type = request.food_type,
                Description = request.Description,
                FoodName = request.FoodName,
                difficulty_level = request.difficulty_level,
                meal_type = request.meal_type,
                NotExistFoodName = request.NotExistFoodName,
                List_Ingriedents = request.List_Ingriedents,
                nationality = request.nationality,
                NumberOfPicture = request.NumberOfPicture,
                primary_source_of_ingredient = request.primary_source_of_ingredient,
                Steps = request.Steps,
                Time = request.Time
            };
            if (request.NotExistFoodName != "" || request.List_Ingriedents.Where(ING => ING.Item4 == false).ToList().Count != 0)
            {
                var result = await _recipeService.CreateInCompleteRecipe(new_request, isExistsUser.Id, finduser.UserName);
                if (result.IsSucceed)
                {
                    return Ok(result.Message);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            var result1 = await _recipeService.CreateRecipeByChef(new_request, isExistsUser.Id, finduser.UserName);
            if (result1.IsSucceed)
            {
                return Ok(result1.Message);

            }
            return Problem(detail: result1.Message, statusCode: 400);
        }
    }
}
