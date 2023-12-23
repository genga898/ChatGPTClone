using ChatGPTClone.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ChatGPTClone.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<ChatOptions> _chatOptions;

        public OpenAIService(HttpClient httpClient, IOptions<ChatOptions> chatOptions)
        {
            _httpClient = httpClient;
            _chatOptions = chatOptions;
        }


        public async Task<Message?> CreateChatCompletion(List<Message> messages)
        {
            var request = new { model = _chatOptions.Value.GptModel, messages = messages.ToArray()};

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _chatOptions.Value.ApiKey);

            var response = await _httpClient.PostAsJsonAsync(_chatOptions.Value.ApiUrl, request);
            response.EnsureSuccessStatusCode();

            var chatCompleteResponse = await response.Content.ReadFromJsonAsync<ChatbotResponse>();
            return chatCompleteResponse?.choices.First().message;
        }
    }
}
