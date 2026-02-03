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

        [ObservableProperty]
        private bool isSourceCollectionVisible;

        public SourceViewModel(INewsService newsService, NewsQueryViewModel query)
        {
            _newsService = newsService;
            _query = query;
        }

        public async Task LoadSourcesAsync(string? countryCode)
        {
            Sources.Clear();
            SelectedSource = null; // Reset valet när källor laddas om
            IsSourceButtonVisible = false;

            if (string.IsNullOrWhiteSpace(countryCode))
            {
                _query.SourceId = null; // Nollställ sourceId när inget land är valt
                return;
            }

            var sources = await _newsService.GetSourcesByCountryAsync(countryCode);

            // Lägg till "Alla källor" som första alternativ
            Sources.Add(new SourceDto { Id = string.Empty, Name = "Alla källor" });

            foreach (var s in sources)
                Sources.Add(s);

            IsSourceButtonVisible = Sources.Any();
        }

        partial void OnSelectedSourceChanged(SourceDto? value)
        {
            // Stäng CollectionView när ett val görs
            IsSourceCollectionVisible = false;

            // Uppdatera query med sourceId (null om "Alla källor" valts)
            _query.SourceId = string.IsNullOrWhiteSpace(value?.Id) ? null : value.Id;
        }
    }
}
