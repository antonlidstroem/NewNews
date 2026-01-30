using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Input;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel
    {
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

            string? categoryFilter =
                SelectedCategory != "Allt" ? SelectedCategory.ToLower() : null;

            string? countryCode = SelectedCountry == "Allt" || string.IsNullOrWhiteSpace(SelectedCountry)
            ? null
            : AvailableCountries[SelectedCountry];

            string? sourceId = SelectedSource?.Id; // null är redan OK

            string? languageCode = LanguageVM.CurrentLanguageCode;
            var news = await _newsService.GetNewsPageAsync(
                currentPage,
                pageSize,
                SearchQuery ?? "nyheter",
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
        public async Task LoadMoreNewsCommand()
        {
            await LoadMoreNews(SearchQuery ?? "nyheter");
        }

        [RelayCommand]
        private void ClearSearch()
        {
            SearchQuery = string.Empty;
        }

    }
}
