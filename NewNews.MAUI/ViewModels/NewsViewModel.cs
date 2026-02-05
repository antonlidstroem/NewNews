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
        private readonly INewsCacheService _cache;

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
            NewsQueryViewModel query,
            INewsCacheService cache)
        {
            _newsService = newsService;
            _browser = browser;
            _query = query;
            _cache = cache;
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
            _cache.ClearExpired(); // Rensa utgången cache vid ny sökning

            await LoadMoreNews();
        }

        [RelayCommand]
        public async Task LoadMoreNews()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine("No internet connection");
                return;
            }

            if (IsBusy || !hasMoreItems) return;

            try
            {
                IsBusy = true;
                System.Diagnostics.Debug.WriteLine($"Loading page {currentPage}...");

                var query = SelectedEndpoint == "everything"
                    ? (string.IsNullOrWhiteSpace(SearchQuery) ? "nyheter" : SearchQuery)
                    : null;

                var cacheKey = _cache.GenerateCacheKey(
                    SelectedEndpoint,
                    query,
                    _query.LanguageCode,
                    _query.SourceId,
                    currentPage);

                List<News> news;

                if (_cache.TryGet(cacheKey, out var cachedNews))
                {
                    news = cachedNews;
                }
                else
                {
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
                        _cache.Set(cacheKey, news);
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
        }
    }
}