using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class NewsViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;
        private readonly IBrowserService _browser;
        private readonly NewsQueryViewModel _query;

        public ObservableCollection<ArticleViewModel> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string selectedEndpoint = "everything";

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

        [RelayCommand]
        public async Task SearchNews()
        {
            if (IsBusy) return;

            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;
            await LoadMoreNews();
        }

        [RelayCommand]
        public async Task LoadMoreNews()
        {
            if (IsBusy || !hasMoreItems) return;

            try
            {
                IsBusy = true;

                var news = await _newsService.GetNewsPageAsync(
                    currentPage,
                    pageSize,
                    _query.SearchQuery ?? "nyheter",
                    _query.LanguageCode,
                    _query.SourceId,
                    SelectedEndpoint);

                if (news == null || news.Count == 0)
                {
                    hasMoreItems = false;
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        foreach (var item in news)
                            Articles.Add(new ArticleViewModel(item));
                    });
                    currentPage++;
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
            _ = SearchNews(); // Sök om när endpoint ändras
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
    }
}