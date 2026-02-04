using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.DAL.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class SavedSearchViewModel : BaseViewModel
    {
        private readonly ISearchKeywordService _keywordService;
        private readonly NewsViewModel _newsVM;
        private readonly LanguageViewModel _languageVM;
        private readonly NewsQueryViewModel _query;

        public ObservableCollection<SavedSearch> SavedKeywords { get; } = new();

        [ObservableProperty] private SavedSearch? selectedSavedSearch;
        [ObservableProperty] public bool areSavedKeywordsVisible;

        public SavedSearchViewModel(ISearchKeywordService keywordService,
                                    NewsViewModel newsVM,
                                    LanguageViewModel languageVM,
                                    NewsQueryViewModel query)
        {
            _keywordService = keywordService;
            _newsVM = newsVM;
            _languageVM = languageVM;
            _query = query;
        }

        [RelayCommand]
        private async Task SaveSearch()
        {
            if (string.IsNullOrWhiteSpace(_newsVM.SearchQuery)) return;

            await _keywordService.AddKeywordAsync(
                _newsVM.SearchQuery,
                _languageVM.SelectedLanguage,
                null);

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
            _languageVM.SelectedLanguage = value.Language ?? "Svenska";
            _ = _newsVM.SearchNews();
        }

        public async Task LoadSavedKeywords()
        {
            var keywords = await _keywordService.GetAllKeywordsAsync();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                SavedKeywords.Clear();

                foreach (var k in keywords)
                    SavedKeywords.Add(k);
            });
        }

    }
}