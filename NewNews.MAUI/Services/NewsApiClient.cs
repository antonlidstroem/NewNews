using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using NewNews.MAUI.Dto;

namespace NewNews.MAUI.Services
{
    public class NewsApiClient : INewsApiClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public NewsApiClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.Timeout = TimeSpan.FromSeconds(30); // Timeout för att förhindra hängningar

            _apiKey = config["NewsApi:ApiKey"] ?? throw new Exception("API key missing from configuration");

            if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _http.DefaultRequestHeaders.Add("User-Agent", "NewNewsApp/1.0");
            }
        }

        public async Task<NewsApiResponseDto?> GetEverythingAsync(
            string query,
            string? language,
            int page,
            int pageSize,
            string? sourceId = null)
        {
            try
            {
                var url = $"https://newsapi.org/v2/everything?q={Uri.EscapeDataString(query)}" +
                    $"&sortBy=publishedAt" +
                    $"&page={page}" +
                    $"&pageSize={pageSize}" +
                    $"&apiKey={_apiKey}";

                if (!string.IsNullOrEmpty(language))
                    url += $"&language={language}";

                if (!string.IsNullOrEmpty(sourceId))
                    url += $"&sources={sourceId}";

                var response = await _http.GetFromJsonAsync<NewsApiResponseDto>(url);
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetEverythingAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<NewsApiResponseDto?> GetTopHeadlinesAsync(
            string? query,
            string? language,
            int page,
            int pageSize,
            string? sourceId = null)
        {
            try
            {
                // Top headlines fungerar lite annorlunda - country ELLER sources (inte båda)
                var url = $"https://newsapi.org/v2/top-headlines?" +
                    $"page={page}" +
                    $"&pageSize={pageSize}" +
                    $"&apiKey={_apiKey}";

                if (!string.IsNullOrEmpty(sourceId))
                {
                    url += $"&sources={sourceId}";
                }
                else if (!string.IsNullOrEmpty(language))
                {
                    // Om inget source, använd språk som filter (fungerar begränsat)
                    url += $"&language={language}";
                }

                if (!string.IsNullOrEmpty(query))
                    url += $"&q={Uri.EscapeDataString(query)}";

                var response = await _http.GetFromJsonAsync<NewsApiResponseDto>(url);
                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTopHeadlinesAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {
            try
            {
                var url = $"https://newsapi.org/v2/top-headlines/sources?apiKey={_apiKey}";
                var response = await _http.GetFromJsonAsync<SourcesResponseDto>(url);
                return response?.Sources ?? new List<SourceDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetSourcesAsync: {ex.Message}");
                return new List<SourceDto>();
            }
        }
    }
}