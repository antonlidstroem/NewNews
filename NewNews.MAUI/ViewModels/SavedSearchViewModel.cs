using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.DAL.Services;
using NewNews.MAUI.ViewModels.Base;


namespace NewNews.MAUI.ViewModels
{
    public partial class SavedSearchViewModel : BaseViewModel
    {
        private readonly SearchKeywordService _keywordService;
        private readonly NewsViewModel _newsVM;
        private readonly LanguageViewModel _languageVM;
        private readonly CategoryViewModel _categoryVM;
        private readonly NewsQueryViewModel _query;

        public ObservableCollection<SavedSearch> SavedKeywords { get; } = new();

        [ObservableProperty] private SavedSearch? selectedSavedSearch;
        [ObservableProperty] public bool areSavedKeywordsVisible;

        public SavedSearchViewModel(SearchKeywordService keywordService,
                                    NewsViewModel newsVM,
                                    LanguageViewModel languageVM,
                                    CategoryViewModel categoryVM,
                                    NewsQueryViewModel query)
        {
            _keywordService = keywordService;
            _newsVM = newsVM;
            _languageVM = languageVM;
            _categoryVM = categoryVM;
            _query = query;
        }

        private string? MapLanguage(string lang) =>
            lang switch
            {
                "svenska" => "sv",
                "english" => "en",
                _ => null
            };

        [RelayCommand]
        private async Task SearchByKeyword(SavedSearch search)
        {
            if (search == null) return;

            _query.SearchQuery = search.Keyword;
            _query.LanguageCode = MapLanguage(search.Language);
            _query.Category = search.Category;
            _newsVM.SelectedCategory = _categoryVM.SelectedCategory;
            await _newsVM.SearchNews();

        }

        [RelayCommand]
        private async Task SaveSearch()
        {
            if (string.IsNullOrWhiteSpace(_newsVM.SearchQuery)) return;

            string? categoryToSave =
            _categoryVM.SelectedCategory != "Allt"
                ? _categoryVM.SelectedCategory
                : null;

            await _keywordService.AddKeywordAsync(
                _newsVM.SearchQuery,
                _languageVM.SelectedLanguage,
                categoryToSave);

            await LoadSavedKeywords();

        }

        [RelayCommand]
        private async Task DeleteSavedSearch(SavedSearch search)
        {
            if (search == null) return;
            await _keywordService.DeleteKeywordAsync(search.Id);
            await LoadSavedKeywords();
        }

        partial void OnSelectedSavedSearchChanged(SavedSearch? value)
        {
            if (value == null) return;

            AreSavedKeywordsVisible = false;

            _newsVM.SearchQuery = value.Keyword;
            _languageVM.SelectedLanguage = value.Language;

            _categoryVM.SelectedCategory =
                string.IsNullOrWhiteSpace(value.Category) ? "Allt" : value.Category;

            _ = _newsVM.SearchNews();

        }

        public async Task LoadSavedKeywords()
        {
            SavedKeywords.Clear();
            var keywords = await _keywordService.GetAllKeywordsAsync();
            foreach (var k in keywords)
                SavedKeywords.Add(k);
        }
    }
}
