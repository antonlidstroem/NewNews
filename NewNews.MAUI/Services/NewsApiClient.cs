using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
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
            //_apiKey = config["NewsApi:ApiKey"] ?? throw new Exception("API key missing");
            _apiKey = "34c60333dc6e4b75823ff4348ac7e12a";

            if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
            {
                _http.DefaultRequestHeaders.Add("User-Agent", "NewNewsApp/1.0");
            }
        }

        public async Task<NewsApiResponseDto?> GetEverythingAsync(
        string query,
        string language,
        int page,
        int pageSize)
        {
            var url =
                $"https://newsapi.org/v2/everything" +
                $"?q={Uri.EscapeDataString(query)}" +
                $"&language={language}" +
                $"&sortBy=publishedAt" +
                $"&page={page}" +
                $"&pageSize={pageSize}" +
                $"&apiKey={_apiKey}";

            return await _http.GetFromJsonAsync<NewsApiResponseDto>(url);
        }

        public async Task<NewsApiResponseDto?> GetTopHeadlinesAsync(string url) =>
        await _http.GetFromJsonAsync<NewsApiResponseDto>(url);

        public async Task<SourcesResponseDto?> GetSourcesAsync(string url) =>
            await _http.GetFromJsonAsync<SourcesResponseDto>(url);

        public async Task<List<SourceDto>> GetSourcesByCountryAsync(string country)
        {
            var url = $"https://newsapi.org/v2/sources?country={country}&apiKey={_apiKey}";
            var response = await GetSourcesAsync(url);
            return response?.Sources ?? new List<SourceDto>();
        }

    }
}
