using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.DAL.Services;
using NewNews.MAUI.Dto;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly SearchKeywordService _keywordService;

        public LanguageViewModel LanguageVM { get; }
        public NewsViewModel NewsVM { get; }
        public CategoryViewModel CategoryVM { get; }
        public SavedSearchViewModel SavedSearchVM { get; }
        public CountryViewModel CountryVM { get; }
        public SourceViewModel SourceVM { get; }

        public UiStateViewModel UiStateVM { get; }

        // Sökfält
        [ObservableProperty] private bool isBusy;

        public MainViewModel(INewsService newsService, SearchKeywordService keywordService, IBrowserService browserService)
        {
            _keywordService = keywordService;
            LanguageVM = new LanguageViewModel();
            NewsVM = new NewsViewModel(newsService, browserService);
            CategoryVM = new CategoryViewModel();
            CountryVM = new CountryViewModel();
            SourceVM = new SourceViewModel();

             

            

        // När språk ändras, uppdatera nyheter
        LanguageVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                {
                    CategoryVM.UpdateVisibility(LanguageVM.SelectedLanguage);
                    NewsVM.LanguageCode = LanguageVM.CurrentLanguageCode;
                    await NewsVM.SearchNews();
                }
            };


            CategoryVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CategoryVM.SelectedCategory))
                {
                    NewsVM.SelectedCategory = CategoryVM.SelectedCategory;
                    await NewsVM.SearchNews();
                }
            };

            SavedSearchVM = new SavedSearchViewModel(
                _keywordService,
                NewsVM,
                LanguageVM,
                CategoryVM);

            UiStateVM = new UiStateViewModel(
                LanguageVM,
                CountryVM,
                CategoryVM,
                SourceVM,
                SavedSearchVM);

            // Country → News
            CountryVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CountryVM.SelectedCountry))
                {
                    NewsVM.SelectedCountry = CountryVM.CurrentCountryCode;
                    await NewsVM.SearchNews();
                }
            };

            // Source → News
            SourceVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(SourceVM.SelectedSource))
                {
                    NewsVM.SelectedSource = SourceVM.SelectedSource;
                    await NewsVM.SearchNews();
                }
            };
        }  

        [RelayCommand]
        private void ClearSearch()
        {
            NewsVM.SearchQuery = string.Empty;
        }


    }
}
