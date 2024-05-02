using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
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
    public class AccountController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IImageProcessorService _imageProcessorService;
        private readonly UserManager<Chef> _userManager;
        private readonly IChefService _chefService;
        private readonly IJwt _jwtService;
        public AccountController(IUserService userService, IImageProcessorService imageProcessorServic, UserManager<Chef> userManager, IChefService chefService, IJwt jwtService)
        {
            _userService = userService;
            _imageProcessorService = imageProcessorServic;
            _userManager = userManager;
            _chefService = chefService;
            _jwtService = jwtService;
        }
        [HttpGet]
        [Route("Read_User")]
        [Authorize]
        public async Task<IActionResult> Read()
        {
            //string token = HttpContext.Request.Headers["Authorization"];
            //string username = _jwtService.DecodeToken(token);
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user = await _userManager.GetUserAsync(currentUser);
            var ReadResult = await _userService.Read(user.UserName);
            if (ReadResult.IsSucceed)
            {
                ApplicationUser readuser = (ApplicationUser)ReadResult.user;
                ReadUserResponse response = new ReadUserResponse
                {
                    ImageUrl = await _imageProcessorService.GetImagebyUser(user.UserName),
                    FullName = readuser.FullName,
                    UserName = readuser.UserName,
                    Email = readuser.Email,
                    PhoneNumber = readuser.PhoneNumber,
                    Aboutme = readuser.Aboutme,
                    Gen=readuser.Gender
                };
                return Ok(response);
            }

            return Problem(detail: ReadResult.Message, statusCode: 400);
        }
        [HttpPost]
        [Route("Update_User")]
        [Authorize]
        public async Task<IActionResult> Update()
        {
            try
            {
                EditUserRequest request = new EditUserRequest
                {
                    PhoneNumber = Request.Form["phoneNumber"],
                    Email = Request.Form["email"],
                    Aboutme = Request.Form["aboutYou"],
                    FullName = Request.Form["fullName"],
                    Gen = Request.Form["gender"]
                };
                //string token = HttpContext.Request.Headers["Authorization"];
                //string username = _jwtService.DecodeToken(token);
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var finduser = await _userManager.GetUserAsync(currentUser);
                var EditResult = await _userService.Edit(request, finduser.UserName);
                if (EditResult.IsSucceed)
                {
                    var file = Request.Form.Files[0];
                    var Result = await _imageProcessorService.UploadUserImage(file, finduser.UserName);
                    if (Result.IsSucceed)
                    {
                        var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
                        ApplicationUser user = (ApplicationUser)isExistsUser;
                        user.ImageURL = await _imageProcessorService.GetImagebyUser(finduser.UserName);
                        ReadUserResponse response = new ReadUserResponse
                        {
                            ImageUrl = await _imageProcessorService.GetImagebyUser(finduser.UserName),
                            FullName = user.FullName,
                            UserName = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            Aboutme = user.Aboutme,
                            Gen = user.Gender
                        };
                        //Result.ImagePath = user.ImageURL;
                        return Ok(new GeneralResponse { Message = EditResult.Message });

                    }

                    return Ok(new GeneralResponse { Message = EditResult.Message });
                }
                return Problem(detail: EditResult.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
 
        [HttpGet]
        [Route("Recent_Recipes_User")]
        [Authorize]
        public async Task<IActionResult> Recent_Recipes_User()
        {
            //string token = HttpContext.Request.Headers["Authorization"];
            //string username = _jwtService.DecodeToken(token);
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _userService.RecentRecipeByUser(isExistsUser.Id);
            List<SummaryRecipeInfoForUserResponse> recentRecipe = new List<SummaryRecipeInfoForUserResponse>();
            foreach (var item in result)
            {

                recentRecipe.Add(new SummaryRecipeInfoForUserResponse
                {
                    Score = item.recipe.Score,
                    ChefName = item.recipe.chef.FullName,
                    ChefUserName = item.recipe.chef.UserName,
                    VisitTime = item.VisitTime,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(item.recipe.food.Name, finduser.UserName,item.recipe.Name, 1),
                    Name = item.recipe.Name
                });

            }

            return Ok(result);
        }
        [HttpGet]
        [Route("Favourite_Recipes_User")]
        [Authorize]
        public async Task<IActionResult> Favourite_Recipes_User()
        {
            //string token = HttpContext.Request.Headers["Authorization"];
            //string username = _jwtService.DecodeToken(token);
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _userService.FavouritRecipeByUser(isExistsUser.Id);
            List<SummaryRecipeInfoForUserResponse> favouriteRecipe = new List<SummaryRecipeInfoForUserResponse>();
            foreach (var item in result)
            {

                favouriteRecipe.Add(new SummaryRecipeInfoForUserResponse
                {
                    Score = item.recipe.Score,
                    ChefName = item.recipe.chef.FullName,
                    ChefUserName = item.recipe.chef.UserName,
                    IsFavorite=true,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(item.recipe.food.Name, finduser.UserName, item.recipe.Name, 1),
                    Name = item.recipe.Name
                });

            }

            return Ok(result);
        }
        [HttpGet]
        [Route("Favourite_Recipes_User_OrderBy_Score")]
        [Authorize]
        public async Task<IActionResult> Favourite_Recipes_User_OrderBy_Score()
        {
            //string token = HttpContext.Request.Headers["Authorization"];
            //string username = _jwtService.DecodeToken(token);
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var finduser = await _userManager.GetUserAsync(currentUser);
            var isExistsUser = await _userManager.FindByNameAsync(finduser.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _userService.FavouritRecipeByUserOrderByScore(isExistsUser.Id);
            List<SummaryRecipeInfoForUserResponse> favouriteRecipe = new List<SummaryRecipeInfoForUserResponse>();
            foreach (var item in result)
            {

                favouriteRecipe.Add(new SummaryRecipeInfoForUserResponse
                {
                    Score = item.recipe.Score,
                    ChefName = item.recipe.chef.FullName,
                    ChefUserName = item.recipe.chef.UserName,
                    IsFavorite = true,
                    ImagePath = await _imageProcessorService.GetImagebyRecipe(item.recipe.food.Name, finduser.UserName, item.recipe.Name, 1),
                    Name = item.recipe.Name
                });

            }
            return Ok(result);
        }
        [HttpGet]
        [Route("Recipe_Chef")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Recipe_Chef()
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
                var AllRecipe = await _chefService.GetRecipesByChefId(isExistsUser.Id);
                List<SummaryRecipeInfoAddedByChefResponse> result = new List<SummaryRecipeInfoAddedByChefResponse>();
                foreach (var item in AllRecipe)
                {
                    var ImageUrlRecipe = await _imageProcessorService.GetImagebyRecipe(item.food.Name, isExistsUser.UserName, item.Name, 1);
                    result.Add(new SummaryRecipeInfoAddedByChefResponse
                    {
                        ChefName=item.chef.FullName,
                        ChefUserName=item.chef.UserName,
                        ImagePath=ImageUrlRecipe,
                        IsAccepted=item.IsAccepted,
                        Name=item.Name,
                        Score=item.Score
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPut("Change_Password")]
        //[Authorize]
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
        [HttpPost("test")]
        public async Task<IActionResult> Change_Passwjord(AddIngredientRequest request)
        {
            //string token = HttpContext.Request.Headers["Authorization"];
            try
            {
                string token = request.Name;
                string username = _jwtService.DecodeToken(token);
                var isExistsUser = await _userManager.FindByNameAsync(username);
                if (isExistsUser == null)
                    return BadRequest(new GeneralResponse { Message = "UserName not Exists" });
                var result = await _userManager.ChangePasswordAsync(isExistsUser, "stringHHJJ123@gmail.com", "stringDD132@gmail.com11");
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                string error_message = "";
                foreach (var error in result.Errors)
                {
                    error_message = error_message + " " + error.Description;
                }
                return BadRequest(new GeneralResponse { Message = error_message });
                return Ok(username);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        //[HttpPost("UploadImage")]
        //public async Task<ActionResult> UploadImage()
        //{
        //    try
        //    {
        //        string username = Request.Form["message"];
        //        //string username = "h2";
        //        var isExistsUser = await _userManager.FindByNameAsync(username);

        //        if (isExistsUser != null)
        //            return Problem(detail: "UserName not Exists", statusCode: 400);
        //        var file = Request.Form.Files[0];
        //        var Result = await _imageProcessorService.UploadUserImage(file, username);
        //        if (Result.IsSucceed)
        //        {
        //            ApplicationUser user = (ApplicationUser)isExistsUser;
        //            user.ImageURL = _imageProcessorService.GetImagebyUser(username);
        //            Result.ImagePath = user.ImageURL;
        //            //Result.ImagePath = _imageProcessorService.GetImagebyUser(username);
        //            return Ok(Result);
        //        }
        //        return Problem(detail: Result.Message, statusCode: 400);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(detail: ex.Message, statusCode: 400);
        //    }
        //}
        //[HttpGet("GetUserImage")]
        //public async Task<ActionResult> GetUserImage(ReadUserRequest request)
        //{
        //    var isExistsUser = await _userManager.FindByNameAsync(request.UserName);
        //    if (isExistsUser != null)
        //        return Problem(detail: "UserName not Exists", statusCode: 400);
        //    ApplicationUser user = (ApplicationUser)isExistsUser;
        //    ImageUrlResponse result = new ImageUrlResponse
        //    {
        //        IsSucceed = true,
        //        ImagePath = user.ImageURL
        //    };
        //    return Ok(result);

        //}
        //[HttpGet]
        //[Route("Recent_Food_User")]
        //[Authorize]
        //public async Task<IActionResult> Recent_Food_User()
        //{
        //    string token = HttpContext.Request.Headers["Authorization"];
        //    string username = _jwtService.DecodeToken(token);
        //    var isExistsUser = await _userManager.FindByNameAsync(username);
        //    if (isExistsUser != null)
        //        return Problem(detail: "UserName not Exists", statusCode: 400);
        //    var result = await _userService.RecentFoodByUser(isExistsUser.Id);
        //    return Ok(result);
        //}
        //[HttpGet]
        //[Route("Favourit_Recipe_User_Sorted_By_Time")]
        //[Authorize]
        //public async Task<IActionResult> Favourit_Recipe_User(ReadUserRequest request)
        //{
        //    string token = HttpContext.Request.Headers["Authorization"];
        //    string username = _jwtService.DecodeToken(token);
        //    var isExistsUser = await _userManager.FindByNameAsync(username);
        //    if (isExistsUser != null)
        //        return Problem(detail: "UserName not Exists", statusCode: 400);
        //    var result = await _userService.FavouritFoodByUser(isExistsUser.Id);
        //    return Ok(result);
        //}
        //[HttpGet]
        //[Route("Favourit_Recipe_User_Sorted_By_Rating")]
        //[Authorize]
        //public async Task<IActionResult> Favourit_Recipe_User(ReadUserRequest request)
        //{
        //    string token = HttpContext.Request.Headers["Authorization"];
        //    string username = _jwtService.DecodeToken(token);
        //    var isExistsUser = await _userManager.FindByNameAsync(username);
        //    if (isExistsUser != null)
        //        return Problem(detail: "UserName not Exists", statusCode: 400);
        //    var result = await _userService.FavouritFoodByUser(isExistsUser.Id);
        //    return Ok(result);
        //}
    }

}
