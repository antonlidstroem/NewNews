using NewNews.DAL.Models;

namespace NewNews.DAL.Services
{
    public interface ISearchKeywordService
    {
        Task InitAsync();
        Task<List<SavedSearch>> GetAllKeywordsAsync();
        Task AddKeywordAsync(string keyword, string? language, string? category);
        Task DeleteKeywordAsync(int id);
    }
}