using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
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
            {"Português", "pt"}
        };

        public ObservableCollection<string> LanguageNames { get; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedLanguage = "Svenska";

        [ObservableProperty]
        private bool isLanguageCollectionVisible;

        public string? CurrentLanguageCode =>
            AvailableLanguages.TryGetValue(SelectedLanguage, out var code) && !string.IsNullOrEmpty(code)
                ? code
                : "sv"; // Default till svenska

        public LanguageViewModel(NewsQueryViewModel query)
        {
            // Lägg till alla språk
            foreach (var lang in AvailableLanguages.Keys)
                LanguageNames.Add(lang);

            _query = query;

            // Sätt default språk
            _query.LanguageCode = "sv";
        }

        partial void OnSelectedLanguageChanged(string value)
        {
            IsLanguageCollectionVisible = false;
            _query.LanguageCode = CurrentLanguageCode;

            System.Diagnostics.Debug.WriteLine($"Language changed to: {value} ({CurrentLanguageCode})");
        }
    }
}