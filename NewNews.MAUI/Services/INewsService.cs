using NewNews.DAL.Models;
using NewNews.MAUI.Dto;

namespace NewNews.MAUI.Services
{
    public interface INewsService
    {
        Task<List<News>> GetNewsPageAsync(
            int page,
            int pageSize,
            string query,
            string? language,
            string? sourceId,
            string endpoint = "everything");

        Task<List<SourceDto>> GetSourcesAsync();
    }
}