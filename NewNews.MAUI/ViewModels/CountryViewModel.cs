using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;
using NewNews.MAUI.Dto;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class CountryViewModel : BaseViewModel
    {
        public Dictionary<string, string> AvailableCountries { get; } = new()
        {
            {"Sverige", "se"},
            {"USA", "us"},
            {"Storbritannien", "gb"},
            {"Tyskland", "de"},
            {"Frankrike", "fr"},
            {"Norge", "no"},
        };
        public ObservableCollection<string> CountryNames { get; } = new();


        [ObservableProperty] private string? selectedCountry = "Allt";
        [ObservableProperty] private bool isCountryCollectionVisible;

        public string? CurrentCountryCode =>
            SelectedCountry != "Allt" &&
            AvailableCountries.TryGetValue(SelectedCountry, out var code)
                ? code
                : null;

        public CountryViewModel()
        {
            CountryNames.Add("Allt");
            foreach (var c in AvailableCountries.Keys)
                CountryNames.Add(c);
        }
    }
}
