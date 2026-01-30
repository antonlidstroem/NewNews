using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.MAUI.Dto;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class NewsViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;
        public ObservableCollection<ArticleViewModel> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private string selectedCategory = "Allt";
        [ObservableProperty] private string? selectedCountry;
        [ObservableProperty] private SourceDto? selectedSource;
        [ObservableProperty] private bool isBusy;

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 15;

        public NewsViewModel(INewsService newsService)
        {
            _newsService = newsService;
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
            string? countryCode = SelectedCountry == "Allt" || string.IsNullOrWhiteSpace(SelectedCountry)
                ? null
                : SelectedCountry?.ToLower(); // eller map från AvailableCountries om du vill

            string? sourceId = SelectedSource?.Id;

            // Språk kommer från MainViewModel.LanguageVM
            string? languageCode = (App.Current.MainPage.BindingContext as MainViewModel)?.LanguageVM.CurrentLanguageCode;

            var news = await _newsService.GetNewsPageAsync(
                currentPage,
                pageSize,
                query ?? "nyheter",
                languageCode,
                categoryFilter,
                countryCode,
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
        private void ToggleNewsExpanded(ArticleViewModel article)
        {
            if (article == null) return;

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

        private double webViewHeight;
        public double WebViewHeight
        {
            get => webViewHeight;
            set => SetProperty(ref webViewHeight, value);
        }


        

    }
}



