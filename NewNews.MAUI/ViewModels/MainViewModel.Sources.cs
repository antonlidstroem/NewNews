using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.MAUI.Dto;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {

        [ObservableProperty] private bool isSourceButtonVisible;
        [ObservableProperty] private bool isSourceCollectionVisible;

        public ObservableCollection<SourceDto> Sources { get; } = new();
        [ObservableProperty] private SourceDto? selectedSource;

        [RelayCommand]
        private async Task ToggleSourceCollection()
        {
            if (SelectedCountry == null)
                return;

            IsSourceCollectionVisible = !IsSourceCollectionVisible;

            if (Sources.Count == 0)
            {
                var countryCode = AvailableCountries[SelectedCountry];
                var sources = await _newsService.GetSourcesByCountryAsync(countryCode);

                Sources.Clear();
                foreach (var s in sources)
                    Sources.Add(s);
            }
        }
        partial void OnSelectedSourceChanged(SourceDto value)
        {
            IsSourceCollectionVisible = false;
            _ = SearchNews();
        }

    }
}
