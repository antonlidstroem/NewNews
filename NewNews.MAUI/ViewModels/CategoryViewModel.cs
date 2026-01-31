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
        [ObservableProperty]
        private bool isCategoryVisible;
        [ObservableProperty]
        private bool isCategoryButtonVisible;
        

        public ObservableCollection<string> Categories { get; } = new()
        {
            "Allt","Business","Entertainment","General","Health","Science","Sports","Technology"
        };
        [ObservableProperty] 
        private string selectedCategory = "Allt";


        public CategoryViewModel()
        {
            isCategoryVisible = false;
            isCategoryButtonVisible = false;
        }

        public void UpdateButtonVisibility(string selectedLanguage)
        {
            IsCategoryButtonVisible = selectedLanguage == "English";
        }
    }
}
