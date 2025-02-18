﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gourmet.Core.Services;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Gourmet.Core.Domain.Other_Object;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Diagnostics.Eventing.Reader;
using Gourmet.Core.DataBase.GourmetDbcontext;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJwt _jwtservice;
        private readonly IUsersService _usersService;
        private readonly IChefService _chefService;
        private readonly UserManager<Chef> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public UsersController(IJwt jwtservice, IUsersService usersService, IChefService chefService, UserManager<Chef> userManager, IConfiguration configuration, AppDbContext db)
        {
            _jwtservice = jwtservice;
            _usersService = usersService;
            _chefService = chefService;
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
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
                    Email = signupResult.user.UserName
                };
                return Ok(signUpResponse);
            }

            return Problem(detail: signupResult.Message, statusCode: 400);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginResult = await _usersService.LoginAsync(request);

            if (loginResult.IsSucceed)
            {
                var claims = _userManager.GetClaimsAsync(loginResult.user).Result.ToList();
                var roles = _userManager.GetRolesAsync(loginResult.user).Result.ToList();
                string Role = "";
                if (roles.Contains("ADMIN"))
                {
                    Role = "ADMIN";
                }
                else if (roles.Contains("CHEF"))
                {
                    Role = "CHEF";
                }
                else
                {
                    Role = "USER";
                }
                claims.Add(new Claim(ClaimTypes.Role, Role));
                
                var token = _jwtservice.CreateJwtToken(loginResult.user,claims);
                
                
                AuthenticationResponse login_response = new AuthenticationResponse
                {
                    Email = loginResult.user.UserName,
                    JWT_Token = token,
                    //    Expiration = Expiration,
                    Period = _configuration["JWT:Expiration_Time"],
                    Role=Role,
                    isPremium = loginResult.user.premium > DateTime.Now ? true : false,
                    premium = loginResult.user.premium
                };
                return Ok(login_response);
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
        
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Authrequest request)
        {
            var EmailResult = await _usersService.Authenticate_Email(request);
            if (EmailResult.IsSucceed)
                return Ok(EmailResult);
            Console.WriteLine("sajdalkjsdhlh**************");
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
                [HttpGet]
        [Route("make-chef-requests")]
        public async Task<IActionResult> MakeChefRequests()
        {
            try
            {
                var List =_db.ChefRequests.ToList();
                return Ok(List);
            }
            catch( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("reject-chef")]
        public async Task<IActionResult> RejectChefRequest([FromBody] RejectRequest reject)
        {
            var IsExistRequest = _db.ChefRequests.Where(x => x.UserName == reject.UserName).FirstOrDefault();
            if (IsExistRequest != null)
            {
                if (reject.Reason != null)
                {
                    var Emailres = _usersService.Email_User(reject.UserName, reject.Reason);
                    _db.ChefRequests.Remove(IsExistRequest);
                }
                else
                    _db.ChefRequests.Remove(IsExistRequest);
            }
            else
                return BadRequest("This Chef does not exist");

            return Ok(IsExistRequest);
        }
    }
}