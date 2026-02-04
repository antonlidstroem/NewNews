using NewNews.DAL.Models;
using NewNews.MAUI.Dto;
using NewNews.MAUI.Services;

namespace NewNews.MAUI.Services
{
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
            string? sourceId,
            string endpoint = "everything")
        {
            try
            {
                List<ArticleDto> articles = new();

                if (endpoint == "top-headlines")
                {
                    // Hämta top headlines (kräver country code)
                    var topResponse = await _client.GetTopHeadlinesAsync(
                        query,
                        language,
                        page,
                        pageSize,
                        sourceId);

                    if (topResponse?.Articles != null)
                        articles = topResponse.Articles;
                }
                else
                {
                    // Hämta everything (default)
                    var everythingResponse = await _client.GetEverythingAsync(
                        query,
                        language,
                        page,
                        pageSize,
                        sourceId);

                    if (everythingResponse?.Articles != null)
                        articles = everythingResponse.Articles;
                }

                // Konvertera till News-objekt
                var result = articles
                    .Where(a => !string.IsNullOrEmpty(a.Title)) // Filtrera bort tomma artiklar
                    .Select(a => new News
                    {
                        Title = a.Title,
                        Description = a.Description,
                        Url = a.Url,
                        ImageUrl = a.UrlToImage,
                        Source = a.Source?.Name,
                        Content = a.Content,
                        PublishedAt = a.PublishedAt
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetNewsPageAsync: {ex.Message}");
                return new List<News>();
            }
        }

        public async Task<List<SourceDto>> GetSourcesAsync()
        {
            try
            {
                return await _client.GetSourcesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetSourcesAsync: {ex.Message}");
                return new List<SourceDto>();
            }
        }
    }
}