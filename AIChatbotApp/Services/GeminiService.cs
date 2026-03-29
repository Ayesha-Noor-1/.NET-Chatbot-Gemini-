using System.Text;
using System.Text.Json;
using AIChatbotApp.Models;

namespace AIChatbotApp.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _model;

        public GeminiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["GeminiSettings:ApiKey"]!;
            _apiUrl = config["GeminiSettings:ApiUrl"]!;
            _model = config["GeminiSettings:Model"]!;
        }

        public async Task<string> GetReplyAsync(List<ChatMessage> history)
        {
            // Build messages in OpenRouter format
            var messages = history.Select(m => new
            {
                role = m.Role == "assistant" ? "assistant" : "user",
                content = m.Content
            }).ToList();

            var requestBody = new
            {
                model = _model,
                messages = messages
            };

            var json = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            // OpenRouter requires Authorization header
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://localhost");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "AIChatbotApp");

            var response = await _httpClient.PostAsync(_apiUrl, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API error {response.StatusCode}: {errorBody}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();

            // Parse OpenRouter response format
            using var doc = JsonDocument.Parse(responseJson);
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return reply ?? "Sorry, I couldn't generate a response.";
        }
    }
}