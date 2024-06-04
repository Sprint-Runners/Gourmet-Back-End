using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OpenAiController : ControllerBase
{
    private readonly OpenAiService _openAiService;

    public OpenAiController(OpenAiService openAiService)
    {
        _openAiService = openAiService;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskQuestionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest("Question cannot be empty");
        }

        var answer = await _openAiService.UseChatGPT(request.Question);
        return Ok(new { Answer = answer });
    }
}

public class AskQuestionRequest
{
    public string Question { get; set; }
}
