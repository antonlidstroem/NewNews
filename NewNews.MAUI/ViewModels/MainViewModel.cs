using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.DAL.Services;
using NewNews.MAUI.ViewModels.Base;

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

            var news = await _newsService.GetNewsPageAsync(currentPage, pageSize, query);

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
        private void ToggleNewsExpanded(News news)
        {
            if (news == null) return;

            // Toggle state
            news.IsExpanded = !news.IsExpanded;
        }


    }
}
