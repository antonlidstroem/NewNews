using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class UiStateViewModel : BaseViewModel
    {
        private readonly LanguageViewModel _language;
        private readonly CountryViewModel _country;
        private readonly CategoryViewModel _category;
        private readonly SourceViewModel _source;
        private readonly SavedSearchViewModel _saved;

        public UiStateViewModel(
        LanguageViewModel language,
        CountryViewModel country,
        CategoryViewModel category,
        SourceViewModel source,
        SavedSearchViewModel saved)
        {
            _language = language;
            _country = country;
            _category = category;
            _source = source;
            _saved = saved;
        }

        private void CloseAll()
        {
            _language.IsLanguageCollectionVisible = false;
            _country.IsCountryCollectionVisible = false;
            _category.IsCategoryVisible = false;
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
        private void ToggleCountries()
        {
            bool newState = !_country.IsCountryCollectionVisible;
            CloseAll();
            _country.IsCountryCollectionVisible = newState;
        }

        [RelayCommand]
        private void ToggleCategories()
        {
            bool newState = !_category.IsCategoryVisible;
            CloseAll();
            _category.IsCategoryVisible = newState;
        }

        [RelayCommand]
        private void ToggleSources()
        {
            CloseAll();
            _source.IsSourceCollectionVisible = true;
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
