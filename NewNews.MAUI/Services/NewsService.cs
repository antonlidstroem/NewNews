using NewNews.DAL.Models;
using NewNews.MAUI.Dto;
using NewNews.MAUI.Services;

public class NewsService : INewsService
{
    private readonly INewsApiClient _client;
    public string Language { get; set; } = "sv";

    public NewsService(INewsApiClient client)
    {
        _client = client;
    }

    public async Task<List<News>> GetNewsPageAsync(
        int page,
        int pageSize,
        string query,
        string language,
        string? category,
        string? country,
        string? sourceId)
    {
        // Om ett land är valt använder vi top-headlines
        if (!string.IsNullOrEmpty(country))
        {
            var response = await _client.GetTopHeadlinesByCountryAsync(country, query, page, pageSize);

            return response?.Articles.Select(a => new News
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                ImageUrl = a.UrlToImage,
                Source = a.Source?.Name,
                Content = a.Content,
                PublishedAt = a.PublishedAt
            }).ToList() ?? new List<News>();
        }

        // Annars använder vi everything
        var everythingResponse = await _client.GetEverythingAsync(query, language, page, pageSize);

        return everythingResponse?.Articles.Select(a => new News
        {
            Title = a.Title,
            Description = a.Description,
            Url = a.Url,
            ImageUrl = a.UrlToImage,
            Source = a.Source?.Name,
            Content = a.Content,
            PublishedAt = a.PublishedAt
        }).ToList() ?? new List<News>();
    }

    public async Task<List<SourceDto>> GetSourcesByCountryAsync(string country)
    {
        return await _client.GetSourcesByCountryAsync(country);
    }
}
