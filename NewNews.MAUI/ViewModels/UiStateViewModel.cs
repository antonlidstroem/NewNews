using CommunityToolkit.Mvvm.Input;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class UiStateViewModel : BaseViewModel
    {
        private readonly LanguageViewModel _language;
        private readonly SourceViewModel _source;
        private readonly SavedSearchViewModel _saved;

        public UiStateViewModel(
            LanguageViewModel language,
            SourceViewModel source,
            SavedSearchViewModel saved)
        {
            _language = language;
            _source = source;
            _saved = saved;
        }

        public void CloseAll()
        {
            _language.IsLanguageCollectionVisible = false;
            _source.IsSourceCollectionVisible = false;
            _saved.AreSavedKeywordsVisible = false;
        }

        [RelayCommand]
        private void ToggleLanguages()
        {
            bool newState = !_language.IsLanguageCollectionVisible;
            CloseAll();
            _language.IsLanguageCollectionVisible = newState;
        }

        [RelayCommand]
        private void ToggleSources()
        {
            bool newState = !_source.IsSourceCollectionVisible;
            CloseAll();
            _source.IsSourceCollectionVisible = newState;
        }

        [RelayCommand]
        private void ToggleSaved()
        {
            bool newState = !_saved.AreSavedKeywordsVisible;
            CloseAll();
            _saved.AreSavedKeywordsVisible = newState;
        }
    }
}