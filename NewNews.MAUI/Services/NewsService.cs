using System.Net.Http.Json;
using NewNews.DAL.Models;
using NewNews.MAUI.Dto;

namespace NewNews.DAL.Services
{
    public class NewsService
    {
        private readonly HttpClient _http;
        private const string ApiKey = "34c60333dc6e4b75823ff4348ac7e12a";

        public NewsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<News>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<News>();

            var encodedQuery = Uri.EscapeDataString(query);

            var url =
                $"https://newsapi.org/v2/everything" +
                $"?q={encodedQuery}" +
                $"&sortBy=publishedAt" +
                $"&language=sv" +
                $"&apiKey={ApiKey}";

            var response = await _http.GetFromJsonAsync<NewsApiResponseDto>(url);

            if (response?.Articles == null)
                return new List<News>();

            return response.Articles
                .Select(a => new News
                {
                    Title = a.Title,
                    Description = a.Description,
                    Url = a.Url,
                    ImageUrl = a.UrlToImage,
                    Source = a.Source?.Name,
                    PublishedAt = a.PublishedAt
                })
                .ToList();
        }





    }
}
