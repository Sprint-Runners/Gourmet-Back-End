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

    public class ChefUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly UserManager<Chef> _userManager;
        private readonly IChefService _chefService;
        private readonly IRecipeService _recipeService;
        private readonly IJwt _jwtService;
        private readonly AppDbContext _db;

        public ChefUserController(IUserService userService, IImageProcessorService imageProcessorServic, UserManager<Chef> userManager, IChefService chefService, IJwt jwtService, AppDbContext db,IRecipeService recipeService)
        {

            _userService = userService;
            _imageProcessorService = imageProcessorServic;
            _userManager = userManager;
            _chefService = chefService;
            _jwtService = jwtService;
            _recipeService= recipeService;
            _db = db;
        }
        [HttpGet]
        [Route("Chef_Check")]
        [Authorize(Roles = StaticUserRoles.USER)]
        public async Task<IActionResult> Chef_Check()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser == null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var roles = _userManager.GetRolesAsync(isExistsUser).Result.ToList();
            if (roles.Contains("CHEF"))
            {
                return Ok(new GenerallResponse { Success = false, Message = "Your request has been approved, Please Login again." });
            }
            var IsExistRequest = _db.ChefRequests.Where(x => x.userId == isExistsUser.Id).FirstOrDefault();
            if (IsExistRequest != null)
            {
                return Ok(new GenerallResponse { Success = false, Message = "Your request has been registered, wait for the admin's confirmation." });
            }
            return Ok(new GenerallResponse { Success = true, Message = "" });
        }
        [HttpPost]
        [Route("Chef_Request")]
        [Authorize(Roles = StaticUserRoles.USER)]
        public async Task<IActionResult> Chef_Request(PromoteChefRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser == null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var roles = _userManager.GetRolesAsync(isExistsUser).Result.ToList();
            if (roles.Contains("CHEF"))
            {
                return Ok(new GenerallResponse { Success = false, Message = "Your request has been approved, Please Login again." });
            }
            var IsExistRequest = _db.ChefRequests.Where(x => x.userId == isExistsUser.Id).FirstOrDefault();
            if (IsExistRequest != null)
            {
                return Ok(new GenerallResponse { Success = false, Message = "Your request has been registered, wait for the admin's confirmation." });
            }
            if(isExistsUser.FullName=="" || isExistsUser.FullName ==null)
            {
                return Ok(new GenerallResponse { Success = false, Message = "Please fill your fullname field befor creating a request." });
            }
            if (isExistsUser.Aboutme == "" || isExistsUser.Aboutme == null)
            {
                return Ok(new GenerallResponse { Success = false, Message = "Please fill your aboutme field befor creating a request." });
            }
            ChefRequest chefRequest = new ChefRequest() { Description = request.Description, userId = isExistsUser.Id,UserName=isExistsUser.UserName };
            _db.ChefRequests.Add(chefRequest);
            _db.SaveChanges();
            return Ok(new GenerallResponse { Success = true, Message = "Your request has been registered, wait for the admin's confirmation." });
        }
        [HttpGet]
        [Route("Recipe_Chef")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Recipe_Chef()
        {
           
                //string token = HttpContext.Request.Headers["Authorization"];
                //string username = _jwtService.DecodeToken(token);
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var finduser = await _userManager.GetUserAsync(currentUser);
                var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
                if (isExistsUser == null)
                    return Problem(detail: "UserName not Exists", statusCode: 400);
                var AllAcceptRecipe = await _chefService.GetAcceptedRecipesByChefId(isExistsUser.Id);
                var NotAcceptRecipe = await _chefService.GetNotAcceptedRecipesByChefId(isExistsUser.Id);
                var AllRejecteRecipe = await _chefService.GetNotRejectedRecipesByChefId(isExistsUser.Id);
                List<SummaryRecipeInfoAddedByChefResponse> result1 = new List<SummaryRecipeInfoAddedByChefResponse>();
                List<SummaryRecipeInfoAddedByChefResponse> result2 = new List<SummaryRecipeInfoAddedByChefResponse>();
                List<SummaryRecipeInfoAddedByChefResponse> result3 = new List<SummaryRecipeInfoAddedByChefResponse>();
                foreach (var item in AllAcceptRecipe)
                {
                    var isExitsFood = _db.Foods.Where(x => x.Id == item.FoodId).FirstOrDefault();
                    var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(isExitsFood.Name, isExistsUser.UserName, item.Name, 1);
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
                        ID=item.Id,
                        ChefName = isExistsUser.FullName,
                        ChefUserName = isExistsUser.UserName,
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
                var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 1);

                result2.Add(new SummaryRecipeInfoAddedByChefResponse
                    {
                        ID = item.Id,
                        ChefName = isExistsUser.FullName,
                        ChefUserName = isExistsUser.UserName,
                        ImagePath = ImageUrlRecipe,
                        IsAccepted = item.IsAccepted,
                        IsRejectedted=item.IsReject,
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
                foreach (var item in AllRejecteRecipe)
                {
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
                        Foodname = isExitsFood.Name;
                var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(Foodname, isExistsUser.UserName, item.Name, 1);

                result3.Add(new SummaryRecipeInfoAddedByChefResponse
                    {
                        ID = item.Id,
                        ChefName = isExistsUser.FullName,
                        ChefUserName = isExistsUser.UserName,
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
                List<Tuple<string,List<SummaryRecipeInfoAddedByChefResponse>>> result = new List<Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>>();
                result.Add(new Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>("Accepted",result1));
                result.Add(new Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>("Not accepted", result2));
                result.Add(new Tuple<string, List<SummaryRecipeInfoAddedByChefResponse>>("Rejected", result3));
                return Ok(result);
           
        }
        [HttpPut]
        [Route("Delete_Recipe")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Delete_Recipe(FindRecipeRequest request)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser == null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var IsExistRecipe=await _recipeService.Search_Recipe(request.FoodName, isExistsUser.UserName, request.RecipeName);
            if(IsExistRecipe==null)
                IsExistRecipe = await _recipeService.Search_InComplete_Recipe(request.FoodName, isExistsUser.UserName, request.RecipeName);
            _db.Recipes.Remove(IsExistRecipe);
            _db.SaveChanges();
            return Ok(new GeneralResponse { Message = "Delete recipe succes" });
        }

    }
}
