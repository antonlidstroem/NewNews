using System.Net.Http.Json;
using NewNews.DAL.Models;
using NewNews.MAUI.Dto;

public class NewsService
{
    private readonly HttpClient _http;
    private const string ApiKey = "34c60333dc6e4b75823ff4348ac7e12a";

    public NewsService(HttpClient http)
    {
        _http = http;
    }

    // Hämta en "sida" nyheter med ett sökord (default: "nyheter")
    public async Task<List<News>> GetNewsPageAsync(int page = 1, int pageSize = 10, string query = "nyheter")
    {
        try
        {
            var encodedQuery = Uri.EscapeDataString(query);
            var url = $"https://newsapi.org/v2/everything?q={encodedQuery}&language=sv&sortBy=publishedAt&pageSize={pageSize}&page={page}&apiKey={ApiKey}";

            var response = await _http.GetFromJsonAsync<NewsApiResponseDto>(url);

            if (response?.Articles == null || response.Articles.Count == 0)
                return new List<News>();

            return response.Articles.Select(a => new News
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                ImageUrl = a.UrlToImage,
                Source = a.Source?.Name,
                PublishedAt = a.PublishedAt
            }).ToList();
        }
        catch (Exception ex)
        {
            // Lägg till logg här om du vill se fel
            Console.WriteLine($"NewsService error: {ex.Message}");
            return new List<News>();
        }
    }


    // Sökning med pagination (tillåter eget sökord)
    public async Task<List<News>> SearchNewsPageAsync(string query, int page = 1, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<News>();

        return await GetNewsPageAsync(page, pageSize, query);
    }
}
