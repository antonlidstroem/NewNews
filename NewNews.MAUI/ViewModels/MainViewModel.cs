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

            LanguageVM = new LanguageViewModel(_query);
            CategoryVM = new CategoryViewModel(_query);
            CountryVM = new CountryViewModel(_query);
            SourceVM = new SourceViewModel(newsService, _query);
            NewsVM = new NewsViewModel(newsService, browserService, _query);
            SavedSearchVM = new SavedSearchViewModel(_keywordService, NewsVM, LanguageVM, CategoryVM, _query);

            UiStateVM = new UiStateViewModel(LanguageVM, CountryVM, CategoryVM, SourceVM, SavedSearchVM);

            _ = SavedSearchVM.LoadSavedKeywords();

            LanguageVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                {
                    CategoryVM.UpdateButtonVisibility(LanguageVM.SelectedLanguage);
                    await OnLanguageChangedAsync();
                }
            };

            CategoryVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CategoryVM.SelectedCategory))
                    await OnCategoryChangedAsync();
            };

            CountryVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CountryVM.SelectedCountry))
                {
                    await OnCountryChangedAsync();
                    await SourceVM.LoadSourcesAsync(CountryVM.SelectedCountry?.Code);
                }
            };
        }

        private Task OnLanguageChangedAsync() => NewsVM.SearchNews();
        private Task OnCategoryChangedAsync() => NewsVM.SearchNews();
        private Task OnCountryChangedAsync() => NewsVM.SearchNews();
    }
}