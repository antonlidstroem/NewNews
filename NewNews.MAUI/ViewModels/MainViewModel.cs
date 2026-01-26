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

        public ObservableCollection<News> Articles { get; } = new();

        [ObservableProperty]
        private string? searchQuery;

        public MainViewModel(NewsService newsService)
        {
            _newsService = newsService;
            Title = "Nyheter";
        }

        [RelayCommand]
        private async Task SearchNews()
        {
            Articles.Clear();

            var results = await _newsService.SearchAsync(SearchQuery ?? "");

            foreach (var news in results)
                Articles.Add(news);
        }
    }
}
