using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public string SignUp()
        {
            return "Hello World";
        }
    }
}
