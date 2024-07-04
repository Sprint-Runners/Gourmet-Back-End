using Gourmet.Core.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using RapidApiExample.Services;
using System.Threading.Tasks;

namespace RapidApiExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiGPTController : ControllerBase
    {
        private readonly RapidApiService _rapidApiService;

        public ApiGPTController(RapidApiService rapidApiService)
        {
            _rapidApiService = rapidApiService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion(QuestionRequest request)
        {
            var response = await _rapidApiService.SendQuestionAsync(request.Question);
            return Ok(response);
        }
    }
}
