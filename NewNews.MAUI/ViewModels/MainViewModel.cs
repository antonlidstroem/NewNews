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
        
        public ObservableCollection<News> Articles { get; } = new();

        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private bool isBusy;

        private bool hasMoreItems = true;
        private int currentPage = 1;
        private const int pageSize = 5;

        public MainViewModel(NewsService newsService, SearchKeywordService keywordService)
        {
            _newsService = newsService;
            _keywordService = keywordService;

            LanguageNames = new ObservableCollection<string>(AvailableLanguages.Keys);
        }



        


    }
}
