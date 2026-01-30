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
        private readonly INewsService _newsService;
        private readonly SearchKeywordService _keywordService;
        
        public ObservableCollection<ArticleViewModel> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 5;

        public LanguageViewModel LanguageVM { get; }

        public MainViewModel(INewsService newsService, SearchKeywordService keywordService)
        {
            _newsService = newsService;
            _keywordService = keywordService;

            LanguageVM = new LanguageViewModel();

            LanguageVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(LanguageVM.SelectedLanguage))
                {
                    _ = SearchNews();
                }
            };

            //LanguageNames = new ObservableCollection<string>(AvailableLanguages.Keys);

            CountryNames = new ObservableCollection<string>();
            CountryNames.Add("Allt");

            foreach (var country in AvailableCountries.Keys)
            {
                CountryNames.Add(country);
            }

        }

        //private async void InitializeAsync()
        //{
        //    await _keywordService.InitAsync();
        //}

        private bool _initialized;

        public async Task EnsureInitializedAsync()
        {
            if (_initialized) return;

            await _keywordService.InitAsync();
            await LoadSavedKeywords();
            _initialized = true;
        }



    

        [RelayCommand]
        private void ToggleLanguageCollection()
        {
            IsCountryCollectionVisible = false;
            IsSourceCollectionVisible = false;
            AreSavedKeywordsVisible = false;

            //IsLanguageCollectionVisible = !IsLanguageCollectionVisible;
        }



    }
}
