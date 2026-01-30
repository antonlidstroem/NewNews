using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.DAL.Services;
using NewNews.MAUI.Dto;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly SearchKeywordService _keywordService;

        public LanguageViewModel LanguageVM { get; }
        public NewsViewModel NewsVM { get; }

        // Länder
        [ObservableProperty] private bool isCountryCollectionVisible;
        [ObservableProperty] private string? selectedCountry;
        public Dictionary<string, string> AvailableCountries { get; } = new()
        {
            {"Sverige", "se"},
            {"USA", "us"},
            {"Storbritannien", "gb"},
            {"Tyskland", "de"},
            {"Frankrike", "fr"},
            {"Norge", "no"},
        };
        public ObservableCollection<string> CountryNames { get; }

        // Källor
        [ObservableProperty] private bool isSourceButtonVisible;
        [ObservableProperty] private bool isSourceCollectionVisible;
        [ObservableProperty] private SourceDto? selectedSource;
        public ObservableCollection<SourceDto> Sources { get; } = new();

        // Sparade sökningar
        public ObservableCollection<SavedSearch> SavedKeywords { get; } = new();
        [ObservableProperty] private SavedSearch? selectedSavedSearch;
        [ObservableProperty] private bool areSavedKeywordsVisible;

        // Kategori
        public ObservableCollection<string> Categories { get; } = new()
        {
            "Allt","Business","Entertainment","General","Health","Science","Sports","Technology"
        };
        [ObservableProperty] private string selectedCategory = "Allt";
        public bool IsCategoryVisible => LanguageVM.SelectedLanguage == "English";

        // Sökfält
        [ObservableProperty] private bool isBusy;

        public MainViewModel(INewsService newsService, SearchKeywordService keywordService)
        {
            _keywordService = keywordService;
            LanguageVM = new LanguageViewModel();
            NewsVM = new NewsViewModel(newsService);

            // När språk ändras, uppdatera nyheter
            LanguageVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                    await NewsVM.SearchNews();
            };

            // Fyll land
            CountryNames = new ObservableCollection<string> { "Allt" };
            foreach (var country in AvailableCountries.Keys)
                CountryNames.Add(country);
        }

        public async Task EnsureInitializedAsync()
        {
            await _keywordService.InitAsync();
            await LoadSavedKeywords();
        }

        private async Task LoadSavedKeywords()
        {
            SavedKeywords.Clear();
            var keywords = await _keywordService.GetAllKeywordsAsync();
            foreach (var k in keywords)
                SavedKeywords.Add(k);
        }

        // --------------------------
        // Commands
        // --------------------------
        [RelayCommand]
        private void ToggleLanguageCollection()
        {
            IsCountryCollectionVisible = false;
            IsSourceCollectionVisible = false;
            AreSavedKeywordsVisible = false;
            LanguageVM.IsLanguageCollectionVisible = !LanguageVM.IsLanguageCollectionVisible;
        }

        [RelayCommand]
        private void ToggleCountryCollection()
        {
            LanguageVM.IsLanguageCollectionVisible = false;
            IsSourceCollectionVisible = false;
            AreSavedKeywordsVisible = false;
            IsCountryCollectionVisible = !IsCountryCollectionVisible;
        }

        [RelayCommand]
        private void SelectCountry(string country)
        {
            SelectedCountry = country;
            IsCountryCollectionVisible = false;

            NewsVM.SelectedCountry = country;
            _ = NewsVM.SearchNews();
        }

        [RelayCommand]
        private void ToggleSourceCollection()
        {
            LanguageVM.IsLanguageCollectionVisible = false;
            IsCountryCollectionVisible = false;
            AreSavedKeywordsVisible = false;

            IsSourceCollectionVisible = !IsSourceCollectionVisible;
        }

        partial void OnSelectedSourceChanged(SourceDto value)
        {
            NewsVM.SelectedSource = value;
            IsSourceCollectionVisible = false;
            _ = NewsVM.SearchNews();
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            NewsVM.SelectedCategory = value;
            _ = NewsVM.SearchNews();
        }

        [RelayCommand]
        private async Task SearchByKeyword(SavedSearch search)
        {
            if (search == null) return;

            NewsVM.SearchQuery = search.Keyword;
            LanguageVM.SelectedLanguage = search.Language;
            SelectedCategory = string.IsNullOrWhiteSpace(search.Category) ? "Allt" : search.Category;

            NewsVM.SearchQuery = NewsVM.SearchQuery;
            NewsVM.SelectedCategory = SelectedCategory;
            await NewsVM.SearchNews();
        }

        [RelayCommand]
        private async Task SaveSearch()
        {
            if (string.IsNullOrWhiteSpace(NewsVM.SearchQuery)) return;

            string? categoryToSave = IsCategoryVisible && SelectedCategory != "Allt" ? SelectedCategory : null;
            await _keywordService.AddKeywordAsync(NewsVM.SearchQuery, LanguageVM.SelectedLanguage, categoryToSave);

            await LoadSavedKeywords();
        }

        [RelayCommand]
        private async Task DeleteSavedSearch(SavedSearch search)
        {
            if (search == null) return;
            await _keywordService.DeleteKeywordAsync(search.Id);
            await LoadSavedKeywords();
        }

        [RelayCommand]
        private void ToggleSavedKeywords()
        {
            LanguageVM.IsLanguageCollectionVisible = false;
            IsCountryCollectionVisible = false;
            IsSourceCollectionVisible = false;

            AreSavedKeywordsVisible = !AreSavedKeywordsVisible;
        }

        [RelayCommand]
        private void ClearSearch()
        {
            NewsVM.SearchQuery = string.Empty;
        }

        [RelayCommand]
        public async Task LoadMoreNewsCommand()
        {
            if (NewsVM != null)
                await NewsVM.LoadMoreNews(NewsVM.SearchQuery ?? "nyheter");
        }

        partial void OnSelectedSavedSearchChanged(SavedSearch? value)
        {
            if (value == null) return;

            NewsVM.SearchQuery = value.Keyword;
            LanguageVM.SelectedLanguage = value.Language;

            if (!string.IsNullOrWhiteSpace(value.Category))
                SelectedCategory = value.Category;
            else
                SelectedCategory = "Allt";

            if (NewsVM != null)
                _ = NewsVM.SearchNews();
        }
    }
}
