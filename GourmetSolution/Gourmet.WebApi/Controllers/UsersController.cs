using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gourmet.Core.Services;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJwt _jwtservice;
        private readonly IUsersService _usersService;
        private readonly IChefService _chefService;
        public UsersController(IJwt jwtservice, IUsersService usersService, IChefService chefService)
        {
            _jwtservice = jwtservice;
            _usersService = usersService;
            _chefService = chefService;
        }
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seerRoles = await _usersService.SeedRolesAsync();

            return Ok(seerRoles.Message);
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var signupResult = await _usersService.Sign_Up_User(request);
            if (signupResult.IsSucceed)
            {

                SignUpResponse signUpResponse = new SignUpResponse
                {
                    Id = Guid.Parse(signupResult.user.Id),
                    Email=signupResult.user.UserName
                };
                return Ok(signUpResponse);
            }

            return Problem(detail:signupResult.Message,statusCode:400);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginResult = await _usersService.LoginAsync(request);

            if (loginResult.IsSucceed)
            {
                var response = _jwtservice.CreateJwtToken(loginResult.user);
                return Ok(response);
            }

            return Unauthorized(loginResult.Message);
        }
        [HttpGet("Test")]
        public async Task<IActionResult> Test(string token)
        {
            if (_jwtservice.Token_Validation(token))
                return Ok();
            else
                return BadRequest();
        }
        // Route -> make user -> admin
        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionRequest updatePermission)
        {
            var operationResult = await _usersService.MakeAdminAsync(updatePermission);

            if (operationResult.IsSucceed)
                return Ok(operationResult.Message);

            return BadRequest(operationResult.Message);
        }

        // Route -> make user -> chef
        [HttpPost]
        [Route("make-chef")]
        public async Task<IActionResult> MakeChef([FromBody] UpdatePermissionRequest updatePermission)
        {
            var operationResult = await _chefService.MakeChefAsync(updatePermission);

            if (operationResult.IsSucceed)
                return Ok(operationResult.Message);

            return BadRequest(operationResult.Message);
        }
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Authrequest request)
        {
            var EmailResult = await _usersService.Authenticate_Email(request);
            if (EmailResult.IsSucceed)
                return Ok(EmailResult);
            
            return Problem(detail: EmailResult.Message, statusCode: 400);
        }
        [HttpPost("Forget")]
        public async Task<IActionResult> Generate_Temp([FromBody] Add_Temp_Password request)
        {
            var EmailResult = await _usersService.Temproary_Password(request);
            if (EmailResult.IsSucceed)
                return Ok(EmailResult);

            return Problem(detail: EmailResult.Message, statusCode: 400);
        }
    }
}

