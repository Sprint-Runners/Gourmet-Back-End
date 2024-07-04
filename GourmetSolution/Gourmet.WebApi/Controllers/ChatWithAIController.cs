using Microsoft.AspNetCore.Mvc;
using RapidApiExample.Services;
using System.Threading.Tasks;

namespace RapidApiExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly RapidApiService _rapidApiService;

        public ApiController(RapidApiService rapidApiService)
        {
            _rapidApiService = rapidApiService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] string question)
        {
            var response = await _rapidApiService.SendQuestionAsync(question);
            return Ok(response);
        }
    }
}
