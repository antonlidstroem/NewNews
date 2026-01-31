using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.Dto;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class NewsViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;
        private readonly IBrowserService _browser;
        public ObservableCollection<ArticleViewModel> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private string selectedCategory = "Allt";
        [ObservableProperty] private SourceDto? selectedSource;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string? languageCode;
        [ObservableProperty] private CountryDto? selectedCountry;


        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 15;

        public NewsViewModel(INewsService newsService, IBrowserService browser)
        {
            _newsService = newsService;
            _browser = browser;
        }

        [RelayCommand]
        public async Task SearchNews()
        {
            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;

            await LoadMoreNews(SearchQuery ?? "nyheter");
        }


        [RelayCommand]
        public async Task LoadMoreNews(string? query = "nyheter")
        {
            if (IsBusy || !hasMoreItems) return;
            IsBusy = true;

            string? categoryFilter = SelectedCategory != "Allt" ? SelectedCategory.ToLower() : null;
            string? sourceId = SelectedSource?.Id;

            var news = await _newsService.GetNewsPageAsync(
                currentPage,
                pageSize,
                query ?? "nyheter",
                LanguageCode,
                categoryFilter,
                SelectedCountry?.Code,   
                sourceId);

            foreach (var item in news)
                Articles.Add(new ArticleViewModel(item));

            if (news.Count == 0)
                hasMoreItems = false;
            else
                currentPage++;

            IsBusy = false;
        }


        [RelayCommand]
        private async Task SourceTapped(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (Application.Current.MainPage is not null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new ArticleWebViewPage(url));
                }
            }
        }

        [RelayCommand]
        private async Task OpenInBrowser(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
                await _browser.OpenAsync(url);
        }

        private double webViewHeight;
        public double WebViewHeight
        {
            get => webViewHeight;
            set => SetProperty(ref webViewHeight, value);
        }


        [RelayCommand]
        private void ClearSearch()
        {
            SearchQuery = string.Empty;
        }


    }
}



