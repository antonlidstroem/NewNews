using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class NewsViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;
        private readonly IBrowserService _browser;
        private readonly NewsQueryViewModel _query;

        private readonly Dictionary<string, List<News>> _cache = new();

        public ObservableCollection<ArticleViewModel> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string selectedEndpoint = "top-headlines";

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 20;

        public NewsViewModel(
            INewsService newsService,
            IBrowserService browser,
            NewsQueryViewModel query)
        {
            _newsService = newsService;
            _browser = browser;
            _query = query;
        }

        public async Task InitializeAsync()
        {
            System.Diagnostics.Debug.WriteLine("=== Initializing NewsViewModel ===");
            await SearchNews();
        }

        [RelayCommand]
        public void ToggleEndpoint()
        {
            SelectedEndpoint = SelectedEndpoint == "everything" ? "top-headlines" : "everything";
            System.Diagnostics.Debug.WriteLine($"Toggled endpoint to: {SelectedEndpoint}");
        }

        [RelayCommand]
        public async Task SearchNews()
        {
            if (IsBusy) return;

            System.Diagnostics.Debug.WriteLine($"=== Starting search with endpoint: {SelectedEndpoint} ===");

            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;
            _cache.Clear();

            await LoadMoreNews();
        }

        [RelayCommand]
        public async Task LoadMoreNews()
        {
            if (IsBusy || !hasMoreItems) return;

            try
            {
                IsBusy = true;
                System.Diagnostics.Debug.WriteLine($"Loading page {currentPage}...");

                var cacheKey = GenerateCacheKey(currentPage);
                List<News> news;

                if (_cache.TryGetValue(cacheKey, out var cachedNews))
                {
                    news = cachedNews;
                    System.Diagnostics.Debug.WriteLine($"Using cached data: {news.Count} articles");
                }
                else
                {
                    var query = SelectedEndpoint == "top-headlines" && string.IsNullOrWhiteSpace(SearchQuery)
                        ? null
                        : SearchQuery ?? "nyheter";

                    System.Diagnostics.Debug.WriteLine($"API Call - Query: '{query}', Language: '{_query.LanguageCode}', Source: '{_query.SourceId}', Endpoint: '{SelectedEndpoint}'");

                    news = await _newsService.GetNewsPageAsync(
                        currentPage,
                        pageSize,
                        query,
                        _query.LanguageCode,
                        _query.SourceId,
                        SelectedEndpoint);

                    if (news != null && news.Count > 0)
                    {
                        _cache[cacheKey] = news;
                        System.Diagnostics.Debug.WriteLine($"Cached {news.Count} articles");
                    }
                }

                if (news == null || news.Count == 0)
                {
                    hasMoreItems = false;
                    System.Diagnostics.Debug.WriteLine("No more articles available");
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var item in news)
                            Articles.Add(new ArticleViewModel(item));
                    });

                    System.Diagnostics.Debug.WriteLine($"Added {news.Count} articles. Total: {Articles.Count}");
                    currentPage++;

                    if (news.Count < pageSize)
                        hasMoreItems = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in LoadMoreNews: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                hasMoreItems = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GenerateCacheKey(int page)
        {
            var query = SearchQuery ?? "default";
            var language = _query.LanguageCode ?? "none";
            var source = _query.SourceId ?? "none";
            return $"{SelectedEndpoint}_{query}_{language}_{source}_{page}";
        }

        [RelayCommand]
        private async Task OpenInBrowser(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
                await _browser.OpenAsync(url);
        }

        [RelayCommand]
        private void ClearSearch()
        {
            SearchQuery = string.Empty;
        }

        partial void OnSearchQueryChanged(string? value)
        {
            _query.SearchQuery = value;
        }

        partial void OnSelectedEndpointChanged(string value)
        {
            _query.Endpoint = value;
            _cache.Clear();
            _ = SearchNews();
        }

        [RelayCommand]
        private async Task OpenArticle(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            await Shell.Current.GoToAsync(
                $"{nameof(ArticleWebViewPage)}?url={Uri.EscapeDataString(url)}"
            );
        }

        public void ClearCache()
        {
            _cache.Clear();
            System.Diagnostics.Debug.WriteLine("Cache cleared");
        }
    }
}