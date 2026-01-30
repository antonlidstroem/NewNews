using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel
    {
        public ObservableCollection<string> Categories { get; } = new()
        {
        "Allt",
        "Business",
        "Entertainment",
        "General",
        "Health",
        "Science",
        "Sports",
        "Technology"
         };

        [ObservableProperty]
        private string selectedCategory = "Allt";


        private async Task OnCategoryChangedAsync()
        {
            Articles.Clear();
            currentPage = 1;
            hasMoreItems = true;

            await LoadMoreNews(SearchQuery ?? "nyheter");
        }


        public bool IsCategoryVisible => LanguageVM.SelectedLanguage == "English";

        partial void OnSelectedCategoryChanged(string value)
        {
            _ = OnCategoryChangedAsync();
        }
    }
}
