using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NewNews.MAUI.ViewModels
{
    public partial class NewsQueryViewModel : ObservableObject
    {
        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private string? category;
        [ObservableProperty] private string? languageCode = "en";
        [ObservableProperty] private string? countryCode;
        [ObservableProperty] private string? sourceId;
    }

}
