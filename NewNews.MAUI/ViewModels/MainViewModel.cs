using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly NewsService _newsService;

        public ObservableCollection<News> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 10;

        public MainViewModel(NewsService newsService)
        {
            _newsService = newsService;
            Title = "Nyheter";
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
        private async Task LoadMoreNewsCommand()
        {
            await LoadMoreNews(SearchQuery ?? "nyheter");
        }
    }
}
