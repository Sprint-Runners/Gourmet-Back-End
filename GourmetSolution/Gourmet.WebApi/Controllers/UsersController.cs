using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gourmet.Core.Services;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.Domain.Entities;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJwt _jwtservice;
        private readonly IUsersService _usersService;
        public UsersController(IJwt jwtservice, IUsersService usersService)
        {
            _jwtservice = jwtservice;
            _usersService = usersService;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<SignUpResponse>> SignUp(SignUpRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
                    return Problem(detail:errors,statusCode:400,title:"Sign Up Error");
                }
                
                User new_user = await _usersService.Sign_Up_User(request);

                return Ok(new_user.ToSignUpResponse());

            }
            catch(Exception exception)
            {
                return Problem(detail: exception.Message, statusCode: 400, title: "Sign up Error");
            }
        }
        [HttpGet("Test")]
        public async Task<IActionResult> Test(string token)
        {
            if (_jwtservice.Token_Validation(token))
                return Ok();
            else
                return BadRequest();
        }
    }
}
