using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel
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
        public ObservableCollection<string> LanguageNames { get; }

        [ObservableProperty]
        private string selectedLanguage = "Swedish";


        partial void OnSelectedLanguageChanged(string value)
        {
            if (AvailableLanguages.TryGetValue(value, out var langCode))
                _newsService.Language = langCode;

            SearchNewsCommand.Execute(null);
            OnPropertyChanged(nameof(IsCategoryVisible));
        }
    }
}
