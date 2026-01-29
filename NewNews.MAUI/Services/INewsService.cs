using NewNews.DAL.Models;
using NewNews.MAUI.Dto;

public interface INewsService
{
    Task<List<News>> GetNewsPageAsync(
        int page,
        int pageSize,
        string query,
        string language,
        string? category,
        string? country,
        string? sourceId);

    Task<List<SourceDto>> GetSourcesByCountryAsync(string country);
}
