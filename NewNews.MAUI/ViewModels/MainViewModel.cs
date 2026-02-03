using NewNews.DAL.Services;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly NewsQueryViewModel _query = new();
        private readonly SearchKeywordService _keywordService;

        public LanguageViewModel LanguageVM { get; }
        public CategoryViewModel CategoryVM { get; }
        public CountryViewModel CountryVM { get; }
        public SourceViewModel SourceVM { get; }
        public NewsViewModel NewsVM { get; }
        public SavedSearchViewModel SavedSearchVM { get; }
        public UiStateViewModel UiStateVM { get; }

        public MainViewModel(INewsService newsService,
                             SearchKeywordService keywordService,
                             IBrowserService browserService)
        {
            _keywordService = keywordService;

            // Skapa ViewModels i rätt ordning
            CategoryVM = new CategoryViewModel(_query);
            LanguageVM = new LanguageViewModel(_query, CategoryVM);
            CountryVM = new CountryViewModel(_query);
            SourceVM = new SourceViewModel(newsService, _query);
            NewsVM = new NewsViewModel(newsService, browserService, _query);
            SavedSearchVM = new SavedSearchViewModel(_keywordService, NewsVM, LanguageVM, CategoryVM, _query);
            UiStateVM = new UiStateViewModel(LanguageVM, CountryVM, CategoryVM, SourceVM, SavedSearchVM);

            // Ladda sparade sökningar
            _ = SavedSearchVM.LoadSavedKeywords();

            // Uppdatera kategoriknappens synlighet baserat på valt språk
            CategoryVM.UpdateButtonVisibility(LanguageVM.SelectedLanguage);

            // Lyssna på språkändringar
            LanguageVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                {
                    await OnLanguageChangedAsync();
                }
            };

            // Lyssna på kategoriändringar
            CategoryVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CategoryVM.SelectedCategory))
                    await OnCategoryChangedAsync();
            };

            // Lyssna på land-ändringar och uppdatera källor
            CountryVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CountryVM.SelectedCountry))
                {
                    await SourceVM.LoadSourcesAsync(CountryVM.SelectedCountry?.Code);
                    await OnCountryChangedAsync();
                }
            };

            // Lyssna på källändringar och sök om
            SourceVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SourceVM.SelectedSource))
                    await OnSourceChangedAsync();
            };
        }

        private Task OnLanguageChangedAsync() => NewsVM.SearchNews();
        private Task OnCategoryChangedAsync() => NewsVM.SearchNews();
        private Task OnCountryChangedAsync() => NewsVM.SearchNews();
        private Task OnSourceChangedAsync() => NewsVM.SearchNews();
    }
}
