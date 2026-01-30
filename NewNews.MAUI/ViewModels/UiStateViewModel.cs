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
            CloseAll();
            _language.IsLanguageCollectionVisible = true;
        }

        [RelayCommand]
        private void ToggleCountries()
        {
            CloseAll();
            _country.IsCountryCollectionVisible = true;
        }

        [RelayCommand]
        private void ToggleCategories()
        {
            CloseAll();
            _category.IsCategoryVisible = true;
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
            CloseAll();
            _saved.AreSavedKeywordsVisible = true;
        }
    }

}
