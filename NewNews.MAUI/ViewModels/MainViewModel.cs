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
        //public SourceViewModel SourceVM { get; }

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
            //SourceVM = new SourceViewModel();


            SavedSearchVM = new SavedSearchViewModel(
                _keywordService,
                NewsVM,
                LanguageVM,
                CategoryVM);

            UiStateVM = new UiStateViewModel(
                LanguageVM,
                CountryVM,
                CategoryVM,
                //SourceVM,
                SavedSearchVM);


            // PRENUMERATIONERING PÅ ÄNDRINGAR I ANDRA VIEWMODELS
            LanguageVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                {
                    _ = OnLanguageChangedAsync();
                }
            };

            CategoryVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CategoryVM.SelectedCategory))
                {
                    _ = OnCategoryChangedAsync();
                }
            };

            CountryVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CountryVM.SelectedCountry))
                {
                    _ = OnCountryChangedAsync();
                }
            };

            //SourceVM.PropertyChanged += async (s, e) =>
            //{
            //    if (e.PropertyName == nameof(SourceVM.SelectedSource))
            //    {
            //       _ = OnSourceChangedAsync();
            //    }
            //};
        }

        private async Task OnLanguageChangedAsync()
        {
            try
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    CategoryVM.UpdateVisibility(LanguageVM.SelectedLanguage);
                    NewsVM.LanguageCode = LanguageVM.CurrentLanguageCode;
                    await NewsVM.SearchNews();
                });
            }
            catch {}
        }

        private async Task OnCategoryChangedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                NewsVM.SelectedCategory = CategoryVM.SelectedCategory;
                await NewsVM.SearchNews();
            });
        }

        private async Task OnCountryChangedAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                NewsVM.SelectedCountry = CountryVM.SelectedCountry;

                    await NewsVM.SearchNews();
            });
        }

        //private async Task OnSourceChangedAsync()
        //{
        //    await MainThread.InvokeOnMainThreadAsync(async () =>
        //    {
        //        NewsVM.SelectedSource = SourceVM.SelectedSource;
        //        await NewsVM.SearchNews();
        //    });
        //}
    }
}
