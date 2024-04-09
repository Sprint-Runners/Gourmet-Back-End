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
using System.IO.Pipelines;

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
        public AccountController(IUserService userService, IImageProcessorService imageProcessorServic, UserManager<Chef> userManager, IChefService chefService)
        {
            _userService = userService;
            _imageProcessorService = imageProcessorServic;
            _userManager = userManager;
            _chefService = chefService;
        }
        [HttpPost]
        [Route("Read_User")]
        public async Task<IActionResult> Read(ReadUserRequest request)
        {
            var ReadResult = await _userService.Read(request);
            if (ReadResult.IsSucceed)
            {
                ApplicationUser readuser = (ApplicationUser)ReadResult.user;
                ReadUserResponse response = new ReadUserResponse
                {
                    FirstNmae = readuser.FirstName,
                    LastNmae = readuser.LastName,
                    UserNmae = readuser.UserName,
                    Email = readuser.Email,
                    PhoneNumber = readuser.PhoneNumber,
                    aboutme = readuser.aboutme
                };
                return Ok(response);
            }

            return Problem(detail: ReadResult.Message, statusCode: 400);
        }
        [HttpPut]
        [Route("Update_User")]
        public async Task<IActionResult> Update(EditUserRequest request)
        {
            var ReadResult = await _userService.Edit(request);
            if (ReadResult.IsSucceed)
            {
                return Ok(ReadResult.Message);
            }

            return Problem(detail: ReadResult.Message, statusCode: 400);
        }
        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage()
        {
            try
            {
                string username = Request.Form["message"];
                //string username = "h2";
                var isExistsUser = await _userManager.FindByNameAsync(username);

                if (isExistsUser != null)
                    return Problem(detail: "UserName not Exists", statusCode: 400);
                var file = Request.Form.Files[0];
                var Result = await _imageProcessorService.UploadUserImage(file, username);
                if (Result.IsSucceed)
                {
                    ApplicationUser user = (ApplicationUser)isExistsUser;
                    user.ImageURL = _imageProcessorService.GetImagebyUser(username);
                    Result.ImagePath = user.ImageURL;
                    Result.ImagePath = _imageProcessorService.GetImagebyUser(username);
                    return Ok(Result);
                }
                return Problem(detail: Result.Message, statusCode: 400);
            }
            catch(Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpGet("GetUserImage")]
        public async Task<ActionResult> GetUserImage(ReadUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            ApplicationUser user = (ApplicationUser)isExistsUser;
            ImageUrlResponse result = new ImageUrlResponse
            {
                IsSucceed = true,
                ImagePath = user.ImageURL
            };
            return Ok(result);

        }
        [HttpGet]
        [Route("Recent_Food_User")]
        public async Task<IActionResult> Recent_Food_User(ReadUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _userService.RecentFoodByUser(isExistsUser.Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("Favourit_Recipe_User")]
        public async Task<IActionResult> Favourit_Recipe_User(ReadUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result = await _userService.FavouritFoodByUser(isExistsUser.Id);
            return Ok(result);
        }
        [HttpGet]
        [Route("Recipe_Chef")]
        [Authorize(Roles = StaticUserRoles.CHEF)]
        public async Task<IActionResult> Recipe_Chef(ReadUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);
            if (isExistsUser != null)
                return Problem(detail: "UserName not Exists", statusCode: 400);
            var result=await _chefService.GetRecipesByChefId(isExistsUser.Id);
            return Ok(result);
        }
    }
}
