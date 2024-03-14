using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gourmet.Core.Services;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJwt _jwtservice;
        public UsersController(IJwt jwtservice)
        {
            _jwtservice = jwtservice;
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<AuthenticationResponse>> SignUp(SignUpRequest request)
        {
            AuthenticationResponse response = _jwtservice.CreateJwtToken(request);
            return Ok(response);
        }
/*        [HttpGet("Test")]
        public async Task<IActionResult> Test(string token)
        {
            if (_jwtservice.Token_Validation(token))
                return Ok();
            else
                return BadRequest();
        }*/
    }
}
