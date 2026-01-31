using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.Dto;
using NewNews.MAUI.ViewModels;
using NewNews.MAUI.ViewModels.Base;


namespace NewNews.MAUI.ViewModels
{
    public partial class SourceViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;
        private readonly NewsQueryViewModel _query;

        public ObservableCollection<SourceDto> Sources { get; } = new();

        [ObservableProperty]
        private SourceDto? selectedSource;

        [ObservableProperty]
        private bool isSourceButtonVisible;
        [ObservableProperty] private bool isSourceCollectionVisible;

        public SourceViewModel(INewsService newsService, NewsQueryViewModel query)
        {
            _newsService = newsService;
            _query = query;

            _query.PropertyChanged += async (_, e) =>
            {
                if (e.PropertyName == nameof(NewsQueryViewModel.CountryCode))
                    await LoadSourcesAsync(_query.CountryCode);
            };
        }



        public async Task LoadSourcesAsync(string? countryCode)
        {
            Sources.Clear();
            IsSourceButtonVisible = false;

            if (string.IsNullOrWhiteSpace(countryCode))
                return;

            var sources = await _newsService.GetSourcesByCountryAsync(countryCode);

            foreach (var s in sources)
                Sources.Add(s);

            IsSourceButtonVisible = Sources.Any();
        }
    }
}