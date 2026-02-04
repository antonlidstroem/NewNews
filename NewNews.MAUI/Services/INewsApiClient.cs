using NewNews.MAUI.Dto;

namespace NewNews.MAUI.Services
{
    public interface INewsApiClient
    {
        Task<NewsApiResponseDto?> GetEverythingAsync(
            string query,
            string? language,
            int page,
            int pageSize,
            string? sourceId = null);

        Task<NewsApiResponseDto?> GetTopHeadlinesAsync(
            string? query,
            string? language,
            int page,
            int pageSize,
            string? sourceId = null);

        Task<List<SourceDto>> GetSourcesAsync();
    }
}