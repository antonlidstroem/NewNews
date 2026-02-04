using CommunityToolkit.Mvvm.ComponentModel;

namespace NewNews.MAUI.ViewModels
{
    public partial class NewsQueryViewModel : ObservableObject
    {
        [ObservableProperty] private string? searchQuery;
        [ObservableProperty] private string? languageCode = "sv";
        [ObservableProperty] private string? sourceId;
        [ObservableProperty] private string endpoint = "everything"; 
    }
}