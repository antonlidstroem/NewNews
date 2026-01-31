using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using NewNews.DAL.Models;
using NewNews.MAUI.ViewModels.Base;


namespace NewNews.MAUI.ViewModels
{
    public partial class CategoryViewModel : BaseViewModel
    {
        public ObservableCollection<string> Categories { get; } = new()
        {
            "Allt","Business","Entertainment","General","Health","Science","Sports","Technology"
        };
        [ObservableProperty] 
        private string selectedCategory = "Allt";

        [ObservableProperty]
        private bool isCategoryVisible;
        [ObservableProperty]
        private bool isCategoryButtonVisible;

        private readonly NewsQueryViewModel _query;

        public CategoryViewModel(NewsQueryViewModel query)
        {
            _query = query;
        }

        public void UpdateButtonVisibility(string selectedLanguage)
        {
            IsCategoryButtonVisible = selectedLanguage == "English";
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            IsCategoryVisible = false;
            _query.Category = value == "Allt" ? null : value.ToLower();
        }
    }
}
