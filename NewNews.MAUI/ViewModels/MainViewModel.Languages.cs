using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {


        public Dictionary<string, string> AvailableLanguages { get; } = new()
    {
        {"English", "en"},
        {"Svenska", "sv"},
        {"Deutsch", "de"},
        {"Español", "es"},
        {"Français", "fr"},
        {"Italiano", "it"},
        {"Nederlands", "nl"},
        {"Norsk", "no"},
        {"Português", "pt"},
        {"Русский", "ru"},
    };

        // Dropdown-visning
        //public ObservableCollection<string> LanguageNames { get; }
        public ObservableCollection<string> LanguageNames { get; } = new ObservableCollection<string>
            {
                "Allt", // lägg till denna som default/null
                "English", "Svenska", "Deutsch", "Español", "Français", "Italiano", "Nederlands", "Norsk", "Português", "Русский"
            };


        [ObservableProperty]
        private string selectedLanguage = "Svenska";

        private string CurrentLanguageCode =>
    AvailableLanguages.TryGetValue(SelectedLanguage, out var code)
        ? code
        : "sv";


        public string LanguageDisplayName => $"{SelectedLanguage}";


        partial void OnSelectedLanguageChanged(string value)
        {
            IsLanguageCollectionVisible = false;

            OnPropertyChanged(nameof(LanguageDisplayName));
            SearchNewsCommand.Execute(null);
            OnPropertyChanged(nameof(IsCategoryVisible));
        }

        [ObservableProperty]
        private bool isLanguageCollectionVisible = false; // default osynlig

        [RelayCommand]
        private void ToggleLanguageCollection()
        {
            IsCountryCollectionVisible = false;
            IsSourceCollectionVisible = false;
            AreSavedKeywordsVisible = false;

            IsLanguageCollectionVisible = !IsLanguageCollectionVisible;
        }

        [RelayCommand]
        private void SelectLanguage(string language)
        {
            SelectedLanguage = language;
            IsLanguageCollectionVisible = false; // göm efter val
        }

    }
}
