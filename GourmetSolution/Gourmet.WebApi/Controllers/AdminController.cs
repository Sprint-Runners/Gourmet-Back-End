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

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IImageProcessorService _imageProcessorService;
        private readonly IIngredientService _ingredientService;
        private readonly IFoodService _foodService;
        private readonly ICategoriesService _categoriesService;
        private readonly IRecipeService _recipeService;

        private readonly IUsersService _usersService;
        private readonly IChefService _chefService;
        private readonly AppDbContext _db;
        private readonly UserManager<Chef> _userManager;
        public AdminController(IImageProcessorService imageProcessorService, IIngredientService ingredientService, IFoodService foodService, ICategoriesService categoriesService, IRecipeService recipeService, IChefService chefService, AppDbContext db, UserManager<Chef> userManager, IUsersService usersService)
        {
            _imageProcessorService = imageProcessorService;
            _ingredientService = ingredientService;
            _foodService = foodService;
            _categoriesService = categoriesService;
            _recipeService = recipeService;
            _usersService = usersService;
            _chefService = chefService;
            _db = db;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("Add_Ingredient")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_Ingredient(AddIngredientRequest request)
        {
            try
            {
                var result = await _ingredientService.Create(request);
                if (result.IsSucceed)
                {
                    return Ok(result.Message);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_Food")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_Food()
        {
            try
            {
                AddFoodRequest request = new AddFoodRequest
                {
                    Name = Request.Form["Name"]
                };
                var result = await _foodService.Create(request);
                if (result.IsSucceed)
                {
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadFoodImage(file, result.food.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.food.ImgeUrl = await _imageProcessorService.GetImagebyFood(result.food.Name);
                        _db.SaveChanges();
                        return Ok(result.food);
                    }
                    return Problem(detail: ResultImage.Message, statusCode: 400);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpGet]
        [Route("Show_All_Users")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Show_All_Users()
        {
            var users = _db.Users.ToList();
            var ShowUsers = new List<UserInfoResponse>();
            foreach (var item in users)
            {
                ShowUsers.Add(new UserInfoResponse
                {
                    Gender = item.Gender,
                    FullName = item.FullName,
                    Aboutme = item.Aboutme,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    UserName = item.UserName,
                    ImageURL = await _imageProcessorService.GetImagebyUser(item.UserName),
                    isPremium = item.premium > DateTime.Now ? true : false,
                    premium = item.premium,
                    isBan=item.Ban,
                    isChef = _userManager.GetRolesAsync((Chef)item).Result.ToList().Contains("CHEF"),
                    requestChef = _db.ChefRequests.Where(x => x.UserName == item.UserName).FirstOrDefault() != null
                });
            }
            return Ok(ShowUsers);
        }
        [HttpPut]
        [Route("Show_Chef")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Show_Chef(GetInformationChefRequest request)
        {
            var isExistsChef = await _userManager.FindByNameAsync(request.ChefUserName);
            if (isExistsChef == null)
            {
                return Problem(detail: "Chef not exist", statusCode: 400);
            }
            if (_userManager.GetRolesAsync(isExistsChef).Result.ToList().Contains("CHEF") == false)
            {
                return Problem(detail: "This user is not chef", statusCode: 400);
            }
            var AllAcceptRecipe = await _chefService.GetAcceptedRecipesByChefId(isExistsChef.Id);
            var NotAcceptRecipe = await _chefService.GetNotAcceptedRecipesByChefId(isExistsChef.Id);
            List<SummaryRecipeInfoAddedByChefResponse> result1 = new List<SummaryRecipeInfoAddedByChefResponse>();
            List<SummaryRecipeInfoAddedByChefResponse> result2 = new List<SummaryRecipeInfoAddedByChefResponse>();
            foreach (var item in AllAcceptRecipe)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExistsChef.UserName, item.Name, 1);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                string Foodname = "";
                if (isExitsFood == null)
                    Foodname = item.FoodString;
                else
                    Foodname = isExitsFood.Name;
                result1.Add(new SummaryRecipeInfoAddedByChefResponse
                {
                    ID = item.Id,
                    ChefName = isExistsChef.FullName,
                    ChefUserName = isExistsChef.UserName,
                    ImagePath = ImageUrlRecipe,
                    IsAccepted = item.IsAccepted,
                    IsRejectedted = item.IsReject,
                    Name = item.Name,
                    Score = item.Score,
                    FoodName = Foodname,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    Description = item.Description,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer
                });
            }
            foreach (var item in NotAcceptRecipe)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExistsChef.UserName, item.Name, 1);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                string Foodname = "";
                if (isExitsFood == null)
                    Foodname = item.FoodString;
                else
                    Foodname = isExitsFood.Name;
                result2.Add(new SummaryRecipeInfoAddedByChefResponse
                {
                    ID = item.Id,
                    ChefName = isExistsChef.FullName,
                    ChefUserName = isExistsChef.UserName,
                    ImagePath = ImageUrlRecipe,
                    IsAccepted = item.IsAccepted,
                    IsRejectedted = item.IsReject,
                    Name = item.Name,
                    Score = item.Score,
                    FoodName = Foodname,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    Description = item.Description,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer
                });
            }
            return Ok(new ChefInfoResponse
            {
                Score = isExistsChef.Score,
                RecipeCount = AllAcceptRecipe.Count(),
                NotAcceptRecipes = result1,
                AcceptRecipes = result2

            });
        }
        [HttpPost]
        [Route("Add_PSOI_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_PSOI_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreatePSOICategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "PSOI", result.PSOI.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.PSOI.ImageUrl = await _imageProcessorService.GetImagebyCategory("PSOI", result.PSOI.Name);
                        return Ok(result.PSOI);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }

        [HttpPost]
        [Route("Add_CM_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_CM_Category(AddCategoryRequest request)
        {
            try
            {
                Console.WriteLine("vkfhfjhgcgjhdcgddhd**********");
                var result = await _categoriesService.CreateCMCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "CM", result.CM.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.CM.ImageUrl = await _imageProcessorService.GetImagebyCategory("CM", result.CM.Name);
                        return Ok(result.CM);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_FT_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_FT_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateFTCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "FT", result.FT.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.FT.ImageUrl = await _imageProcessorService.GetImagebyCategory("FT", result.FT.Name);
                        return Ok(result.FT);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_N_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_N_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateNCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "N", result.N.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.N.ImageUrl = await _imageProcessorService.GetImagebyCategory("N", result.N.Name);
                        return Ok(result.N);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_MT_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_MT_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateMTCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "MT", result.MT.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.MT.ImageUrl = await _imageProcessorService.GetImagebyCategory("MT", result.MT.Name);
                        return Ok(result.MT);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_DL_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_DL_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateDLCategory(request);
                if (result.IsSucceed)
                {
                    return Ok(result.DL);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Accept_recipe")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Accept_recipe(AcceptedRecipeRequest request)
        {
            try
            {
                var result = await _recipeService.AcceptedRecipe(request.FoodName, request.username, request.Name);
                if (result.IsSucceed)
                {
                    return Ok(new GeneralResponse
                    {
                        Message = "Recipe Accepted"
                    });
                }
                return Ok(new GenerallResponse { Success = false, Message = result.Message });
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Reject_recipe")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Reject_recipe(AcceptedRecipeRequest request)
        {
            try
            {
                var result = await _recipeService.RejectedRecipe(request.FoodName, request.username, request.Name);
                if (result.IsSucceed)
                {
                    return Ok(new GeneralResponse
                    {
                        Message = "Recipe Rejected"
                    });
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPut]
        [Route("BanUser")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Ban_User_ByUsername(BanUserRequest request)
        {

            try
            {
                var result = await _usersService.BanUser(request);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPut]
        [Route("UnBanUser")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> UnBan_User_ByUsername(BanUserRequest request)
        {

            try
            {
                var result = await _usersService.UnBanUser(request);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Accept_Chef")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> AcceptChef([FromBody] UpdatePermissionRequest updatePermission)
        {
            var IsExistRequest = _db.ChefRequests.Where(x => x.UserName == updatePermission.UserName).FirstOrDefault();
            if (IsExistRequest != null)
            {
                _db.ChefRequests.Remove(IsExistRequest);
            }
            var operationResult = await _chefService.MakeChefAsync(updatePermission);
            _db.SaveChanges();
            if (operationResult.IsSucceed)
                return Ok(operationResult.Message);

            return BadRequest(operationResult.Message);
        }
        [HttpPost]
        [Route("Reject_Chef")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> RejectChef([FromBody] UpdatePermissionRequest updatePermission)
        {
            var IsExistRequest = _db.ChefRequests.Where(x => x.UserName == updatePermission.UserName).FirstOrDefault();
            if (IsExistRequest != null)
            {
                _db.ChefRequests.Remove(IsExistRequest);
                _db.SaveChanges();
            }
            return Ok(new GeneralResponse
            {
                Message = "Chef Rejected"
            });

        }
        [HttpPut]
        [Route("Show_Recipe_Info")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Show_Recipe_Info(ShowRecipeForAdminOrChefRequest request)
        {
            var item = _db.Recipes.Where(x => x.Id == request.ID).FirstOrDefault();
            var isExistsUser = _db.Users.Where(x => x.Id == item.ChefId).FirstOrDefault();
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
                Foodname = item.FoodString;
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

            var steps = _db.RecipeSteps.Where(x => x.RecipeId == item.Id).OrderBy(x => x.Number).Select(x => new Tuple<string, string>(x.Number.ToString(), x.explenation)).ToList();
            var notExistingredient = item.NotExistIngredients.Split('.');
            List<Tuple<string, string, string, bool>> List_Ingriedents = new List<Tuple<string, string, string, bool>>();
            foreach (var ing in notExistingredient)
            {
                if (ing != "")
                {
                    var ingr = ing.Split(',');
                    var isExitsIngredient = _db.Ingredients.Where(x => x.Name == ingr[0]).FirstOrDefault();
                    if (isExitsIngredient != null)
                    {
                        List_Ingriedents.Add(new Tuple<string, string, string, bool>(ingr[0], ingr[1], ingr[2], true));
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
            var Recipeingredients = _db.RecipeIngredients.Where(x => x.RecipeId == item.Id).ToList();
            foreach (var i in Recipeingredients)
            {
                var isExitsIngredient = _db.Ingredients.Where(x => x.Id == i.IngredientId).FirstOrDefault();
                List_Ingriedents.Add(new Tuple<string, string, string, bool>(isExitsIngredient.Name, i.Quantity.ToString(), i.Unit, true));
            }
            var NotExistingredient = List_Ingriedents.Where(x => x.Item4 == false).ToList();
            var Existingredient = List_Ingriedents.Where(x => x.Item4 == true).ToList();
            var NotExistIngredients = "";
            foreach (var i in NotExistingredient)
            {
                string ingredient = i.Item1 + "," + i.Item2 + "," + i.Item3;
                NotExistIngredients = NotExistIngredients + ingredient + ".";
            }
            item.NotExistIngredients = NotExistIngredients;
            Console.WriteLine("it is Not Es", NotExistIngredients, "hhhhhhhhhhhhhhhhhhhhhhhhh");
            if (NotExistIngredients == "")
            {
                Console.WriteLine("im herererererrererrre555555555");
            }
            if (item.FoodString == "" && item.NotExistIngredients.Count() != 0)
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
                NotExistFoodName = item.FoodString,
                cooking_method = isExitsCM.Name,
                difficulty_level = isExistDL.Name,
                Description = item.Description,
                food_type = isExitsFT.Name,
                meal_type = isExitsMT.Name,
                nationality = isExitsN.Name,
                Time = item.Time.ToString(),
                primary_source_of_ingredient = isExitsPSOI.Name,
                List_Ingriedents = List_Ingriedents,
                Steps = steps,
            });
        }
        [HttpGet]
        [Route("Show_All_Ingredient")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Show_All_Ingredient()
        {
            var AllIngredients = _db.Ingredients.ToList();
            return Ok(AllIngredients);
        }
        [HttpGet]
        [Route("Show_All_Food")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Show_All_Food()
        {
            var AllFoods = _db.Foods.ToList();
            return Ok(AllFoods);
        }
        [HttpGet]
        [Route("Show_All_Recipe")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Show_All_Recipe()
        {
            var AllAcceptRecipe = _db.Recipes.Where(x=>x.IsReject==false && x.IsAccepted==true).ToList();
            var NotAcceptRecipe = _db.Recipes.Where(x => x.IsReject == false && x.IsAccepted == false).ToList();
            List<SummaryRecipeInfoAddedByChefResponse> result1 = new List<SummaryRecipeInfoAddedByChefResponse>();
            List<SummaryRecipeInfoAddedByChefResponse> result2 = new List<SummaryRecipeInfoAddedByChefResponse>();
            foreach (var item in AllAcceptRecipe)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var Chef = _db.Users.Where(x => x.Id == item.ChefId).FirstOrDefault();
                var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name,Chef.UserName, item.Name, 1);
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                string Foodname = "";
                if (isExitsFood == null)
                    Foodname = item.FoodString;
                else
                    Foodname = isExitsFood.Name;
                result1.Add(new SummaryRecipeInfoAddedByChefResponse
                {
                    ID = item.Id,
                    ChefName = Chef.FullName,
                    ChefUserName = Chef.UserName,
                    ImagePath = ImageUrlRecipe,
                    IsAccepted = item.IsAccepted,
                    IsRejectedted = item.IsReject,
                    Name = item.Name,
                    Score = item.Score,
                    FoodName = Foodname,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    Description = item.Description,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer
                });
            }
            foreach (var item in NotAcceptRecipe)
            {
                var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                var Chef = _db.Users.Where(x => x.Id == item.ChefId).FirstOrDefault();
                var allPSOI = _db.PSOIs.ToList();
                var isExitsPSOI = allPSOI.Where(x => x.Id == item.Primary_Source_of_IngredientId).FirstOrDefault();
                var isExitsCM = _db.CMs.Where(x => x.Id == item.Cooking_MethodId).FirstOrDefault();
                var isExitsFT = _db.FTs.Where(x => x.Id == item.Food_typeId).FirstOrDefault();
                var isExitsN = _db.Ns.Where(x => x.Id == item.NationalityId).FirstOrDefault();
                var isExitsMT = _db.MTs.Where(x => x.Id == item.Meal_TypeId).FirstOrDefault();
                var isExistDL = _db.DLs.Where(x => x.Id == item.Difficulty_LevelId).FirstOrDefault();
                string Foodname = "";
                if (isExitsFood == null)
                    Foodname = item.FoodString;
                else
                    Foodname = isExitsFood.Name;
                var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(Foodname, Chef.UserName, item.Name, 1);

                result2.Add(new SummaryRecipeInfoAddedByChefResponse
                {
                    ID = item.Id,
                    ChefName = Chef.FullName,
                    ChefUserName = Chef.UserName,
                    ImagePath = ImageUrlRecipe,
                    IsAccepted = item.IsAccepted,
                    IsRejectedted = item.IsReject,
                    Name = item.Name,
                    Score = item.Score,
                    FoodName = Foodname,
                    CMName = isExitsCM.Name,
                    DLName = isExistDL.Name,
                    Description = item.Description,
                    FTName = isExitsFT.Name,
                    MTName = isExitsMT.Name,
                    NName = isExitsN.Name,
                    Time = item.Time,
                    PSOIName = isExitsPSOI.Name,
                    CountRate = item.Number_Scorer
                });
            }
           
            List<Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>> result = new List<Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>>();
            result.Add(new Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>("Accepted", result1));
            result.Add(new Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>("Not accepted", result2));
            return Ok(result);
        }
        [HttpPut("Change_Password")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Change_Password(ChangePasswordRequest request)

        {
            try
            {
                //string token = Request.Headers["Authorization"];
                //string username = _jwtService.DecodeToken(token);
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var finduser = await _userManager.GetUserAsync(currentUser);
                var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
                if (isExistsUser == null)
                {
                    GeneralResponse response = new GeneralResponse { Message = "UserName not Exists" };
                    return BadRequest(response);
                }
                var result = await _userManager.ChangePasswordAsync(isExistsUser, request.OldPassword, request.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                string error_message = "";
                foreach (var error in result.Errors)
                {
                    error_message = error_message + " " + error.Description;
                }
                GeneralResponse response1 = new GeneralResponse { Message = error_message };
                return BadRequest(response1);
            }
            catch (Exception ex)
            {
                GeneralResponse response2 = new GeneralResponse { Message = ex.Message };
                return BadRequest(response2);
            }
        }
        //ino vase recipe haee ke kamel naboode bayad benevisi va kamel koni
        //[HttpPost]
        //[Route("Add_Ingredient")]
        //[Authorize(Roles = StaticUserRoles.CHEF)]
        //public async Task<IActionResult> Add_Ingredient(AddIngredientRequest request)
        //{

        //}
    }
}