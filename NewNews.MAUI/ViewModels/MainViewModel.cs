using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.DAL.Services;
using NewNews.MAUI.ViewModels.Base;
//using Microsoft.Maui.Essentials;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly NewsService _newsService;
        private readonly SearchKeywordService _keywordService;
        public ObservableCollection<SavedSearch> SavedKeywords { get; } = new();
        public ObservableCollection<News> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 5;

        public ObservableCollection<string> Categories { get; } = new()
        {
        "Allt",
        "Business",
        "Entertainment",
        "General",
        "Health",
        "Science",
        "Sports",
        "Technology"
         };

        [ObservableProperty]
        private string selectedCategory = "Allt";

        public MainViewModel(NewsService newsService, SearchKeywordService keywordService)
        {
            _newsService = newsService;
            _keywordService = keywordService;
            Title = "Nyheter";
            LoadSavedKeywords();

        }

        [RelayCommand]
        private async Task SearchNews()
        {
            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;

            await LoadMoreNews(SearchQuery ?? "nyheter");
        }

        public async Task LoadMoreNews(string? query = "nyheter")
        {
            if (IsBusy || !hasMoreItems) return;
            IsBusy = true;

            string? categoryFilter = SelectedCategory != "Allt" ? SelectedCategory.ToLower() : null;

            var news = await _newsService.GetNewsPageAsync(currentPage, pageSize, query, categoryFilter);

            foreach (var item in news)
                Articles.Add(item);

            if (news.Count == 0)
            {
                hasMoreItems = false;
            }
            else
            {
                currentPage++;
            }


            IsBusy = false;
        }

        [RelayCommand]
        public async Task LoadMoreNewsCommand()
        {
            await LoadMoreNews(SearchQuery ?? "nyheter");
        }

        private async void LoadSavedKeywords()
        {
            SavedKeywords.Clear();
            var keywords = await _keywordService.GetAllKeywordsAsync();
            foreach (var k in keywords)
                SavedKeywords.Add(k);
        }

        [RelayCommand]
        private async Task SearchByKeyword(SavedSearch search)
        {
            if (search == null) return;

            SearchQuery = search.Keyword;
            await SearchNews();
        }


        [RelayCommand]
        private void ToggleNewsExpanded(News article)
        {
            if (article == null) return;

            // Om du vill att bara en artikel kan vara öppen åt gången:
            foreach (var a in Articles)
                if (a != article)
                    a.IsExpanded = false;

            article.IsExpanded = !article.IsExpanded;
        }

        [RelayCommand]
        private async Task SourceTapped(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                // Navigera till WebView-sidan
                if (Application.Current.MainPage is not null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new ArticleWebViewPage(url));
                }
            }
        }

        [RelayCommand]
        private async Task OpenInBrowser(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            _ = OnCategoryChangedAsync();
        }

        private async Task OnCategoryChangedAsync()
        {
            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;

            await LoadMoreNews(SearchQuery ?? "nyheter");
        }




    }
}
