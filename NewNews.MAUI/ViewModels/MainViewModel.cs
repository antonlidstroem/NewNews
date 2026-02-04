using NewNews.DAL.Services;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly NewsQueryViewModel _query = new();
        private readonly SearchKeywordService _keywordService;

        public LanguageViewModel LanguageVM { get; }
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
            SourceVM = new SourceViewModel(newsService, _query);
            NewsVM = new NewsViewModel(newsService, browserService, _query);
            SavedSearchVM = new SavedSearchViewModel(_keywordService, NewsVM, LanguageVM, _query);
            UiStateVM = new UiStateViewModel(LanguageVM, SourceVM, SavedSearchVM);

            _ = SavedSearchVM.LoadSavedKeywords();

            LanguageVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                    await NewsVM.SearchNews();
            };

            SourceVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SourceVM.SelectedSource))
                    await NewsVM.SearchNews();
            };
        }
    }
}