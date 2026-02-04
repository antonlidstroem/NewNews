using System.ComponentModel;
using NewNews.DAL.Services;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;


namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly NewsQueryViewModel _query = new();
        private readonly ISearchKeywordService _keywordService;

        public LanguageViewModel LanguageVM { get; }
        public SourceViewModel SourceVM { get; }
        public NewsViewModel NewsVM { get; }
        public SavedSearchViewModel SavedSearchVM { get; }
        public UiStateViewModel UiStateVM { get; }

        public MainViewModel(INewsService newsService,
                             ISearchKeywordService keywordService,
                             IBrowserService browserService)
        {
            _keywordService = keywordService;

            LanguageVM = new LanguageViewModel(_query);
            SourceVM = new SourceViewModel(newsService, _query);
            NewsVM = new NewsViewModel(newsService, browserService, _query);
            SavedSearchVM = new SavedSearchViewModel(_keywordService, NewsVM, LanguageVM, _query);
            UiStateVM = new UiStateViewModel(LanguageVM, SourceVM, SavedSearchVM);

            //_ = SavedSearchVM.LoadSavedKeywords();

            // Lyssna på språkändringar
            //LanguageVM.PropertyChanged += async (s, e) =>
            //{
            //    if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
            //    {
            //        // Filtrera källor baserat på nytt språk
            //        SourceVM.FilterSourcesByLanguage(LanguageVM.CurrentLanguageCode);

            //        NewsVM.ClearCache();
            //        await NewsVM.SearchNews();
            //    }
            //};

            // Lyssna på källändringar
            //SourceVM.PropertyChanged += async (s, e) =>
            //{
            //    if (e.PropertyName == nameof(SourceVM.SelectedSource))
            //    {
            //        NewsVM.ClearCache();
            //        await NewsVM.SearchNews();
            //    }
            //};
            LanguageVM.PropertyChanged += OnLanguageChanged;
            SourceVM.PropertyChanged += OnSourceChanged;


        }

        public async Task InitializeAsync()
        {
            await _keywordService.InitAsync(); 
            await SavedSearchVM.LoadSavedKeywords();
            await NewsVM.InitializeAsync();
        }

        private async void OnLanguageChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
            {
                SourceVM.FilterSourcesByLanguage(LanguageVM.CurrentLanguageCode);

                NewsVM.ClearCache();
                await NewsVM.SearchNews();
            }
        }

        private async void OnSourceChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SourceVM.SelectedSource))
            {
                NewsVM.ClearCache();
                await NewsVM.SearchNews();
            }
        }


    }
}