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
        
        public ObservableCollection<CountryDto> Countries { get; } = new();

        [ObservableProperty] 
        private CountryDto? selectedCountry;

        [ObservableProperty] 
        private bool isCountryCollectionVisible;
        public string? CurrentCountryCode => SelectedCountry?.Code;

        private readonly NewsQueryViewModel _query;


        public CountryViewModel(NewsQueryViewModel query)
        {
            _query = query;
            Countries.Add(new CountryDto { Name = "Allt", Code = string.Empty });
            Countries.Add(new CountryDto { Name = "Sverige", Code = "se" });
            Countries.Add(new CountryDto { Name = "USA", Code = "us" });
            Countries.Add(new CountryDto { Name = "Storbritannien", Code = "gb" });
            Countries.Add(new CountryDto { Name = "Tyskland", Code = "de" });
            Countries.Add(new CountryDto { Name = "Frankrike", Code = "fr" });
            Countries.Add(new CountryDto { Name = "Norge", Code = "no" });
        }
        partial void OnSelectedCountryChanged(CountryDto? value)
        {
            IsCountryCollectionVisible = false;
            _query.CountryCode = value?.Code;
        }
    }
}
