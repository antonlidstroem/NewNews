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
        private readonly NewsQueryViewModel _query;
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
        private bool isLanguageCollectionVisible;

        public string? CurrentLanguageCode =>
        AvailableLanguages.TryGetValue(SelectedLanguage, out var code)
            ? code
            : null;

        public LanguageViewModel(NewsQueryViewModel query)
        {
            LanguageNames.Add("Allt");
            foreach (var lang in AvailableLanguages.Keys)
                LanguageNames.Add(lang);
            _query = query;
        }

        partial void OnSelectedLanguageChanged(string value)
        {
            IsLanguageCollectionVisible = false;
            _query.LanguageCode = CurrentLanguageCode;

        }
    }
}
