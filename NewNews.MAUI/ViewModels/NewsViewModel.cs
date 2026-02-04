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

        // Cache för att undvika onödiga API-anrop
        private readonly Dictionary<string, List<News>> _cache = new();
        private string _lastCacheKey = string.Empty;

        public ObservableCollection<ArticleViewModel> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string selectedEndpoint = "top-headlines";

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 20;

        public ObservableCollection<string> Endpoints { get; } = new()
        {
            "everything",
            "top-headlines"
        };

        public NewsViewModel(
            INewsService newsService,
            IBrowserService browser,
            NewsQueryViewModel query)
        {
            _newsService = newsService;
            _browser = browser;
            _query = query;
        }

        // Initialisera med initial sökning
        public async Task InitializeAsync()
        {
            await SearchNews();
        }

        [RelayCommand]
        public async Task SearchNews()
        {
            if (IsBusy) return;

            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;

            // Rensa cache för denna sökning
            _lastCacheKey = string.Empty;

            await LoadMoreNews();
        }

        [RelayCommand]
        public async Task LoadMoreNews()
        {
            if (IsBusy || !hasMoreItems) return;

            try
            {
                IsBusy = true;

                // Skapa cache key baserat på sökparametrar
                var cacheKey = GenerateCacheKey(currentPage);

                List<News> news;

                // Kolla om vi har data i cache
                if (_cache.TryGetValue(cacheKey, out var cachedNews))
                {
                    news = cachedNews;
                    System.Diagnostics.Debug.WriteLine($"Using cached data for page {currentPage}");
                }
                else
                {
                    // Använd tom sträng för top-headlines om SearchQuery är null/tom
                    var query = SelectedEndpoint == "top-headlines" && string.IsNullOrWhiteSpace(SearchQuery)
                        ? null  // Skicka null för att få alla top headlines
                        : SearchQuery ?? "nyheter";

                    news = await _newsService.GetNewsPageAsync(
                        currentPage,
                        pageSize,
                        query,
                        _query.LanguageCode,
                        _query.SourceId,
                        SelectedEndpoint);

                    // Spara i cache
                    if (news != null && news.Count > 0)
                    {
                        _cache[cacheKey] = news;
                        System.Diagnostics.Debug.WriteLine($"Cached {news.Count} articles for page {currentPage}");
                    }
                }

                if (news == null || news.Count == 0)
                {
                    hasMoreItems = false;

                    if (Articles.Count == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("No articles found");
                    }
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var item in news)
                            Articles.Add(new ArticleViewModel(item));
                    });

                    currentPage++;

                    // Om vi fick färre artiklar än pageSize, finns troligen inga fler
                    if (news.Count < pageSize)
                    {
                        hasMoreItems = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading news: {ex.Message}");
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

            // Rensa cache när endpoint ändras
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

        // Rensa cache när filter ändras
        public void ClearCache()
        {
            _cache.Clear();
            System.Diagnostics.Debug.WriteLine("Cache cleared");
        }
    }
}