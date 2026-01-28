using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
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

        public Dictionary<string, string> AvailableLanguages { get; } = new()
    {
        {"English", "en"},
        {"Svenska", "sv"},
        {"Deutsch", "de"},
        {"Español", "es"},
        {"Français", "fr"},
        {"Italiano", "it"},
        {"Nederlands", "nl"},
        {"Norsk", "no"},
        {"Português", "pt"},
        {"Русский", "ru"},
    };

        // Dropdown-visning
        public ObservableCollection<string> LanguageNames { get; }

        [ObservableProperty]
        private string selectedLanguage = "Swedish"; 

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

            LanguageNames = new ObservableCollection<string>(AvailableLanguages.Keys);

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



        private async Task OnCategoryChangedAsync()
        {
            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;

            await LoadMoreNews(SearchQuery ?? "nyheter");
        }

        partial void OnSelectedLanguageChanged(string value)
        {
            if (AvailableLanguages.TryGetValue(value, out var langCode))
                _newsService.Language = langCode;

            SearchNewsCommand.Execute(null); 
            OnPropertyChanged(nameof(IsCategoryVisible));
        }

        public bool IsCategoryVisible => SelectedLanguage == "English";

        partial void OnSelectedCategoryChanged(string value)
        {
            _ = OnCategoryChangedAsync();
        }

        [RelayCommand]
        private async Task SaveSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            string? categoryToSave = IsCategoryVisible && SelectedCategory != "Allt" ? SelectedCategory : null;

            await _keywordService.AddKeywordAsync(SearchQuery, SelectedLanguage, categoryToSave);

            // Ladda om sparade sökningar
            LoadSavedKeywords();
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
            LoadSavedKeywords();
        }

        private int _currentSavedIndex = 0;

        [RelayCommand]
        private void NextSavedSearch()
        {
            if (SavedKeywords.Count == 0) return;

            _currentSavedIndex++;
            if (_currentSavedIndex >= SavedKeywords.Count)
                _currentSavedIndex = 0; 

            SelectSavedSearchByIndex(_currentSavedIndex);
        }

        [RelayCommand]
        private void PreviousSavedSearch()
        {
            if (SavedKeywords.Count == 0) return;

            _currentSavedIndex--;
            if (_currentSavedIndex < 0)
                _currentSavedIndex = SavedKeywords.Count - 1; 

            SelectSavedSearchByIndex(_currentSavedIndex);
        }

        private void SelectSavedSearchByIndex(int index)
        {
            if (index < 0 || index >= SavedKeywords.Count) return;

            var saved = SavedKeywords[index];
            SelectedSavedSearch = saved;
        }

    }
}
