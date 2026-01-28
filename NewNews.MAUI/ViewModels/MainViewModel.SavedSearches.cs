using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        public ObservableCollection<SavedSearch> SavedKeywords { get; } = new();

        private async Task LoadSavedKeywords()
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
        private async Task SaveSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            string? categoryToSave = IsCategoryVisible && SelectedCategory != "Allt" ? SelectedCategory : null;

            await _keywordService.AddKeywordAsync(SearchQuery, SelectedLanguage, categoryToSave);

            // Ladda om sparade sökningar
            await LoadSavedKeywords();
        }

        [ObservableProperty]
        private SavedSearch? selectedSavedSearch;

        partial void OnSelectedSavedSearchChanged(SavedSearch? value)
        {
            if (value == null) return;

            SearchQuery = value.Keyword;
            SelectedLanguage = value.Language;

            if (!string.IsNullOrWhiteSpace(value.Category))
                SelectedCategory = value.Category;
            else
                SelectedCategory = "Allt";

            _ = SearchNews();
        }



        [RelayCommand]
        private async Task DeleteSavedSearch(SavedSearch search)
        {
            if (search == null) return;

            await _keywordService.DeleteKeywordAsync(search.Id);
            await LoadSavedKeywords();
        }

        [ObservableProperty]
        private bool areSavedKeywordsVisible = false;

        [RelayCommand]
        private void ToggleSavedKeywords()
        {
            if (!AreSavedKeywordsVisible)
                IsLanguageCollectionVisible = false;

            AreSavedKeywordsVisible = !AreSavedKeywordsVisible;
        }



    }
}
