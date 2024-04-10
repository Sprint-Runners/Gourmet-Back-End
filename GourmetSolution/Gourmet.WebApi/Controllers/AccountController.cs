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
        [HttpPost]
        [Route("Read_User")]
        [Authorize]
        public async Task<IActionResult> Read()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            string username = _jwtService.DecodeToken(token);
            var ReadResult = await _userService.Read(username);
            if (ReadResult.IsSucceed)
            {
                ApplicationUser readuser = (ApplicationUser)ReadResult.user;
                ReadUserResponse response = new ReadUserResponse
                {
                    ImageUrl = _imageProcessorService.GetImagebyUser(username),
                    FullName = readuser.FullName,
                    UserName = readuser.UserName,
                    Email = readuser.Email,
                    PhoneNumber = readuser.PhoneNumber,
                    Aboutme = readuser.Aboutme
                };
                return Ok(response);
            }

            return Problem(detail: ReadResult.Message, statusCode: 400);
        }
        [HttpPut]
        [Route("Update_User")]
        [Authorize]
        public async Task<IActionResult> Update(EditUserRequest request)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"];
                string username = _jwtService.DecodeToken(token);
                var EditResult = await _userService.Edit(request, username);
                if (EditResult.IsSucceed)
                {
                    var file = Request.Form.Files[0];
                    var Result = await _imageProcessorService.UploadUserImage(file, username);
                    if (Result.IsSucceed)
                    {
                        var isExistsUser = await _userManager.FindByNameAsync(username);
                        ApplicationUser user = (ApplicationUser)isExistsUser;
                        user.ImageURL = _imageProcessorService.GetImagebyUser(username);
                        ReadUserResponse response = new ReadUserResponse
                        {
                            ImageUrl = _imageProcessorService.GetImagebyUser(username),
                            FullName = user.FullName,
                            UserName = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            Aboutme = user.Aboutme
                        };
                        //Result.ImagePath = user.ImageURL;
                        return Ok(Response);
                    }
                    return Problem(detail: Result.Message, statusCode: 400);
                }

                return Problem(detail: EditResult.Message, statusCode: 400);
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
        [HttpGet]
        [Route("Food_User")]
        [Authorize]
        public async Task<IActionResult> Recent_and_Favourit_Food_User()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            string username = _jwtService.DecodeToken(token);
            var isExistsUser = await _userManager.FindByNameAsync(username);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result1 = await _userService.RecentFoodByUser(isExistsUser.Id);
            var result2 = await _userService.FavouritFoodByUser(isExistsUser.Id);
            Dictionary<string, IEnumerable<Food>> result = new Dictionary<string, IEnumerable<Food>>();
            result.Add("Recent_Recipe_User", result1);
            result.Add("Favourit_Recipe_User", result2);

            return Ok(result);
        }
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
        [HttpGet]
        [Route("Recipe_Chef")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Recipe_Chef()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            string username = _jwtService.DecodeToken(token);
            var isExistsUser = await _userManager.FindByNameAsync(username);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _chefService.GetRecipesByChefId(isExistsUser.Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("Change_Password")]
        [Authorize]
        public async Task<IActionResult> Change_Password(ChangePasswordRequest request)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            string username = _jwtService.DecodeToken(token);
            var isExistsUser = await _userManager.FindByNameAsync(username);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _userManager.ChangePasswordAsync(isExistsUser, request.OldPassword, request.OldPassword);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            string error_message = "";
            foreach(var error in result.Errors)
            {
                error_message= error_message +" "+error.Description;
            }
            return Problem(detail: error_message, statusCode: 400);
        }
    }
}
