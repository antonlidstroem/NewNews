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
            _http.Timeout = TimeSpan.FromSeconds(30);

            _apiKey = config["NewsApi:ApiKey"] ?? throw new Exception("API key missing from configuration");

            if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _http.DefaultRequestHeaders.Add("User-Agent", "NewNewsApp/1.0");
            }
        }

        public async Task<NewsApiResponseDto?> GetEverythingAsync(
            string? query,
            string? language,
            int page,
            int pageSize,
            string? sourceId = null)
        {
            try
            {
                // Everything-endpoint kräver alltid en query
                var searchQuery = string.IsNullOrWhiteSpace(query) ? "nyheter" : query;

                var url = $"https://newsapi.org/v2/everything?q={Uri.EscapeDataString(searchQuery)}" +
                    $"&sortBy=publishedAt" +
                    $"&page={page}" +
                    $"&pageSize={pageSize}" +
                    $"&apiKey={_apiKey}";

                if (!string.IsNullOrEmpty(language))
                    url += $"&language={language}";

                if (!string.IsNullOrEmpty(sourceId))
                    url += $"&sources={sourceId}";

                System.Diagnostics.Debug.WriteLine($"Everything URL: {url.Replace(_apiKey, "***")}");

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
                var url = $"https://newsapi.org/v2/top-headlines?" +
                    $"page={page}" +
                    $"&pageSize={pageSize}" +
                    $"&apiKey={_apiKey}";

                // Top headlines kan fungera MED eller UTAN query
                // Men det kräver antingen sources ELLER country/language

                if (!string.IsNullOrEmpty(sourceId))
                {
                    // Om vi har en specifik källa, använd den
                    url += $"&sources={sourceId}";
                }
                else if (!string.IsNullOrEmpty(language))
                {
                    // Om vi har språk, konvertera till country code
                    var country = LanguageToCountry(language);
                    if (!string.IsNullOrEmpty(country))
                    {
                        url += $"&country={country}";
                    }
                }
                else
                {
                    // Fallback till US om inget annat anges
                    url += "&country=us";
                }

                // Lägg till query om den finns
                if (!string.IsNullOrEmpty(query))
                    url += $"&q={Uri.EscapeDataString(query)}";

                System.Diagnostics.Debug.WriteLine($"Top Headlines URL: {url.Replace(_apiKey, "***")}");

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

        // Helper för att konvertera språk till country code
        private string? LanguageToCountry(string language)
        {
            return language?.ToLower() switch
            {
                "sv" => "se",
                "en" => "us",
                "de" => "de",
                "es" => "es",
                "fr" => "fr",
                "it" => "it",
                "nl" => "nl",
                "no" => "no",
                "pt" => "pt",
                "ru" => "ru",
                _ => null
            };
        }
    }
}