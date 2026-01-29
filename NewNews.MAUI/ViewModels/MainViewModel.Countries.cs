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
        [ObservableProperty] private bool isCountryCollectionVisible;
        [ObservableProperty] private string? selectedCountry;




        public Dictionary<string, string> AvailableCountries { get; } = new()
            {
                {"Sverige", "se"},
                {"USA", "us"},
                {"Storbritannien", "gb"},
                {"Tyskland", "de"},
                {"Frankrike", "fr"},
                {"Norge", "no"},
            };

        public ObservableCollection<string> CountryNames { get; }


        [RelayCommand]
        private void ToggleCountryCollection()
        {
            IsCountryCollectionVisible = !IsCountryCollectionVisible;
            IsSourceCollectionVisible = false;
        }

        partial void OnSelectedCountryChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            // Stäng dropdown
            IsCountryCollectionVisible = false;

            // Visa knapp för källor
            IsSourceButtonVisible = true;

            // Nollställ vald källa
            SelectedSource = null;
            Sources.Clear();

            // Reset paginering
            currentPage = 1;
            hasMoreItems = true;

            // Töm artiklar och starta ny sökning
            Articles.Clear();

            _ = SearchNews(); // eller LoadMoreNews direkt
        }


        [RelayCommand]
        private void SelectCountry(string country)
        {
            SelectedCountry = country;
            IsCountryCollectionVisible = false; // stäng dropdown
        }


    }
}
