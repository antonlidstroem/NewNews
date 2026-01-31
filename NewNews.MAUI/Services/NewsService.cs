using NewNews.DAL.Models;
using NewNews.MAUI.Dto;
using NewNews.MAUI.Services;

public class NewsService : INewsService
{
    private readonly INewsApiClient _client;

    public NewsService(INewsApiClient client)
    {
        _client = client;
    }

    public async Task<List<News>> GetNewsPageAsync(
    int page,
    int pageSize,
    string query,
    string? language,
    string? category,
    string? country,
    string? sourceId)
    {
        List<ArticleDto> articles = new();

        if (!string.IsNullOrEmpty(country))
        {
            // HÄMTAR FRÅN TOPHEADLINES
            var topResponse = await _client.GetTopHeadlinesByCountryAsync(
                country, 
                query, 
                page, 
                pageSize);

            if (topResponse?.Articles != null)
                articles = topResponse.Articles;
        }
        else
        {
            // HÄMTAR FRÅN EVERYTHING (PRIMÄR)
            var everythingResponse = await _client.GetEverythingAsync(
                query,
                language,
                page,
                pageSize,
                sourceId: null,
                category: null);

            if (everythingResponse?.Articles != null)
                articles = everythingResponse.Articles;
        }

        // Manuella filter

        if (!string.IsNullOrEmpty(category))
        {
            articles = articles
                .Where(a => a.Category?.Equals(category, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
        }

        if (!string.IsNullOrEmpty(sourceId))
            articles = articles.Where(a => string.Equals(a.Source?.Id, sourceId, StringComparison.OrdinalIgnoreCase)).ToList();

        var result = articles.Select(a => new News
        {
            Title = a.Title,
            Description = a.Description,
            Url = a.Url,
            ImageUrl = a.UrlToImage,
            Source = a.Source?.Name,
            Content = a.Content,
            PublishedAt = a.PublishedAt
        }).ToList();

        return result;
    }


    

    public async Task<List<SourceDto>> GetSourcesByCountryAsync(string country)
    {
        return await _client.GetSourcesByCountryAsync(country);
    }
}
