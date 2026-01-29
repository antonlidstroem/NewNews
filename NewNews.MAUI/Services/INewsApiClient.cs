using NewNews.MAUI.Dto;

namespace NewNews.MAUI.Services
{
    public interface INewsApiClient
    {
        Task<NewsApiResponseDto?> GetEverythingAsync(string query, string language, int page, int pageSize);
        Task<List<SourceDto>> GetSourcesByCountryAsync(string country);
    }

}