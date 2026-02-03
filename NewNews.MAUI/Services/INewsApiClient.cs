using NewNews.MAUI.Dto;

namespace NewNews.MAUI.Services
{
    public interface INewsApiClient
    {
        Task<NewsApiResponseDto?> GetEverythingAsync(string query, string language, int page, int pageSize, string? sourceId = null, string? category = null);
        Task<NewsApiResponseDto?> GetTopHeadlinesByCountryAsync(string country, string? query, int page, int pageSize);
        Task<List<SourceDto>> GetSourcesByCountryAsync(string country);
    }

}
