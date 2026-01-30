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
        private void ToggleSourceCollection()
        {
            LanguageVM.IsLanguageCollectionVisible = false;
            IsCountryCollectionVisible = false;
            AreSavedKeywordsVisible = false;

            IsSourceCollectionVisible = !IsSourceCollectionVisible;
        }
        partial void OnSelectedSourceChanged(SourceDto value)
        {
            IsSourceCollectionVisible = false;
            _ = SearchNews();
        }

    }
}
