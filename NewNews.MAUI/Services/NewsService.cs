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
        var response = await _client.GetEverythingAsync(
            query,
            language,
            page,
            pageSize);

        if (response?.Articles == null)
            return new List<News>();

        return response.Articles.Select(a => new News
        {
            Title = a.Title,
            Description = a.Description,
            Url = a.Url,
            ImageUrl = a.UrlToImage,
            Source = a.Source?.Name,
            Content = a.Content,
            PublishedAt = a.PublishedAt
        }).ToList();
    }

    public async Task<List<SourceDto>> GetSourcesByCountryAsync(string country)
    {
        return await _client.GetSourcesByCountryAsync(country);
    }


}
