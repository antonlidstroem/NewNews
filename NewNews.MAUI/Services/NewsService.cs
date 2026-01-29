using System.Net.Http.Json;
using NewNews.DAL.Models;
using NewNews.MAUI.Dto;

public class NewsService : INewsService
{
    private readonly HttpClient _http;
    private const string ApiKey = "34c60333dc6e4b75823ff4348ac7e12a";
    public string Language { get; set; } = "sv";

    public NewsService(HttpClient http)
    {
        _http = http;

        if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _http.DefaultRequestHeaders.Add("User-Agent", "NewNewsApp/1.0");
        }
    }

    // Hämta en "sida" nyheter med ett sökord (default: "nyheter")
    public async Task<List<News>> GetNewsPageAsync(
            int page,
            int pageSize,
            string query,
            string? category,
            string? country,
            string? sourceId)
            {
                try
                {
                    string url;

                    // 1️⃣ Om källa är vald – DEN VINNER ALLTID
                    if (!string.IsNullOrEmpty(sourceId))
                    {
                        url = $"https://newsapi.org/v2/top-headlines" +
                              $"?sources={sourceId}" +
                              $"&pageSize={pageSize}" +
                              $"&page={page}" +
                              $"&apiKey={ApiKey}";
                    }
                    // 2️⃣ Annars: top-headlines med land / kategori
                    else if (!string.IsNullOrEmpty(country))
                    {
                        url = $"https://newsapi.org/v2/top-headlines" +
                              $"?country={country}" +
                              $"&pageSize={pageSize}" +
                              $"&page={page}" +
                              $"&apiKey={ApiKey}";

                        if (!string.IsNullOrEmpty(category))
                            url += $"&category={category}";
                    }
                    // 3️⃣ Fallback: everything
                    else
                    {
                        url = $"https://newsapi.org/v2/everything" +
                              $"?q={Uri.EscapeDataString(query)}" +
                              $"&language={Language}" +
                              $"&sortBy=publishedAt" +
                              $"&pageSize={pageSize}" +
                              $"&page={page}" +
                              $"&apiKey={ApiKey}";
                    }

                    var response = await _http.GetFromJsonAsync<NewsApiResponseDto>(url);

                    if (response?.Articles == null)
                        return new();

                    return response.Articles.Select(a => new News
                    {
                        Title = a.Title,
                        Description = a.Description,
                        Url = a.Url,
                        ImageUrl = a.UrlToImage,
                        Source = a.Source?.Name,
                        Content = a.CleanContent,
                        PublishedAt = a.PublishedAt
                    }).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"NewsService error: {ex.Message}");
                    return new();
                }
            }





    //// Sökning med pagination (tillåter eget sökord)
    //public async Task<List<News>> SearchNewsPageAsync(string query, int page = 1, int pageSize = 10)
    //{
    //    if (string.IsNullOrWhiteSpace(query))
    //        return new List<News>();

    //    return await GetNewsPageAsync(page, pageSize, query, null, null, null);
    //}

    public async Task<List<SourceDto>> GetSourcesByCountryAsync(string country)
    {
        var url = $"https://newsapi.org/v2/top-headlines/sources?country={country}&apiKey={ApiKey}";
        var response = await _http.GetFromJsonAsync<SourcesResponseDto>(url);

        return response?.Sources ?? new();
    }

}
