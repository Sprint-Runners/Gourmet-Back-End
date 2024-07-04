using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RapidApiExample.Services
{
    public class RapidApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "fe9f522ff1msh552e007fa4dc617p1d16e4jsn264deb2a2716";
        private readonly string _apiHost = "chatgpt-42.p.rapidapi.com";

        public RapidApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendQuestionAsync(string question)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://chatgpt-42.p.rapidapi.com/conversationgpt4-2"),
                Headers =
                {
                    { "x-rapidapi-key", _apiKey },
                    { "x-rapidapi-host", _apiHost },
                },
                Content = new StringContent("{\"messages\":[{\"role\":\"user\",\"content\":\"" + question + "\"}],\"system_prompt\":\"\",\"temperature\":0.9,\"top_k\":5,\"top_p\":0.9,\"max_tokens\":256,\"web_access\":false}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }
}
