using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class LanguageViewModel : BaseViewModel
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

        public ObservableCollection<string> LanguageNames { get; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedLanguage = "Svenska";

        [ObservableProperty]
        private bool isLanguageCollectionVisible = false;

        public string CurrentLanguageCode =>
               SelectedLanguage != null && SelectedLanguage != "Allt" && AvailableLanguages.TryGetValue(SelectedLanguage, out var code)
               ? code : null;

        public string LanguageDisplayName => SelectedLanguage;

        public LanguageViewModel()
        {
            LanguageNames.Add("Allt");
            foreach (var lang in AvailableLanguages.Keys)
                LanguageNames.Add(lang);
        }

        partial void OnSelectedLanguageChanged(string value)
        {
            // Dölj collection view när ett språk väljs
            IsLanguageCollectionVisible = false;
        }

        
    }
}
