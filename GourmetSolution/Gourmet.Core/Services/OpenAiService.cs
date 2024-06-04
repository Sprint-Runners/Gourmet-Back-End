using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OpenAiService
{
    private readonly OpenAIAPI _openai;
    private readonly CompletionRequest completionRequest;

    public OpenAiService(string apiKey)
    {
        _openai = new OpenAIAPI(apiKey);
        completionRequest = new CompletionRequest
        {
            Model = "text-embedding-3-smalls",
        MaxTokens = 100
        };
    }
    public async Task<string> UseChatGPT(string query)
    {
        string outputResult = "";
        completionRequest.Prompt = query;

        var completions = await _openai.Completions.CreateCompletionAsync(completionRequest);

        foreach (var completion in completions.Completions)
        {
            outputResult += completion.Text;
        }

        return outputResult;
    }
    //public async Task<string> AskQuestionAsync(string question)
    //{
    //    var requestBody = new
    //    {
    //        model = "gpt-3.5-turbo", 
    //        prompt = question,
    //        max_tokens = 1500
    //    };

    //    var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
    //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

    //    var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

    //    if (response.IsSuccessStatusCode)
    //    {
    //        var responseContent = await response.Content.ReadFromJsonAsync<JsonDocument>();
    //        var result = responseContent.RootElement.GetProperty("choices")[0].GetProperty("text").GetString();
    //        return result;
    //    }
    //    else
    //    {

    //        var errorContent = await response.Content.ReadAsStringAsync();
    //        throw new ApplicationException($"Error: {response.StatusCode}, Details: {errorContent}");
    //    }
    //}
}
