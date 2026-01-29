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
        List<News> result = new();

        if (!string.IsNullOrEmpty(country))
        {
            // Land är satt → trumfa språk, använd top-headlines
            var topResponse = await _client.GetTopHeadlinesByCountryAsync(country, query, page, pageSize);

            if (topResponse?.Articles != null)
            {
                result = topResponse.Articles.Select(a => new News
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
        }
        else
        {
            // Land är inte satt → använd everything med valt språk
            var everythingResponse = await _client.GetEverythingAsync(query, language, page, pageSize);

            if (everythingResponse?.Articles != null)
            {
                result = everythingResponse.Articles.Select(a => new News
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
        }

        return result;
    }


    public async Task<List<SourceDto>> GetSourcesByCountryAsync(string country)
    {
        return await _client.GetSourcesByCountryAsync(country);
    }
}
