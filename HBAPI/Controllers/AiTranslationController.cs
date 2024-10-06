using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiTranslationController : ControllerBase  
    {
        private readonly ILogger<AiTranslationController> _logger;
        private readonly string _openAiApiKey;

        public AiTranslationController(ILogger<AiTranslationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _openAiApiKey = configuration["OpenAI:ApiKey"];  
        }

        // POST: api/AiTranslation/TranslateText
        [HttpPost("TranslateText")]
        public async Task<IActionResult> TranslateText([FromBody] TranslationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TextToTranslate) || string.IsNullOrWhiteSpace(request.TargetLanguage))
            {
                _logger.LogWarning("Invalid input data received in TranslateText");
                return BadRequest(new { success = false, message = "Invalid input data" });
            }

            try
            {
                _logger.LogInformation($"Received translation request: TextToTranslate: {request.TextToTranslate}, TargetLanguage: {request.TargetLanguage}");

                string translatedText = await TranslateUsingOpenAiAsync(request.TextToTranslate, request.TargetLanguage);
                _logger.LogInformation(translatedText);
                
                return Ok(new { success = true, translatedText, message = "Translation completed successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in TranslateText: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        private async Task<string> TranslateUsingOpenAiAsync(string text, string targetLang)
        {
            string prompt = $"Translate the following text to {targetLang}: {text}";

            using (HttpClient client = new HttpClient())
            {
                // Add OpenAI Authorization Header
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _openAiApiKey);

                // Prepare the request content for OpenAI's chat completion API
                var requestContent = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "You are a translation assistant." },
                        new { role = "user", content = prompt }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

                // Make a POST request to OpenAI's API
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                string result = await response.Content.ReadAsStringAsync();

                // Log the entire response
                _logger.LogInformation($"OpenAI API response: {result}");

                try
                {
                    // Parse the response and return the translation
                    dynamic jsonResponse = JsonConvert.DeserializeObject(result);
                    return jsonResponse.choices[0].message.content.ToString();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error parsing OpenAI API response: {ex.Message}");
                    throw new Exception("Error parsing OpenAI response.");
                }
            }
        }

        // DTO for handling requests
        public class TranslationRequest
        {
            public string TextToTranslate { get; set; } 
            public string TargetLanguage { get; set; }
        }
    }
}
