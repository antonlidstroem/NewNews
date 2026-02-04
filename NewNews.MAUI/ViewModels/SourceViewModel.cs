using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using NewNews.MAUI.Dto;
using NewNews.MAUI.Services;
using NewNews.MAUI.ViewModels.Base;

namespace NewNews.MAUI.ViewModels
{
    public partial class SourceViewModel : BaseViewModel
    {
        private readonly INewsService _newsService;
        private readonly NewsQueryViewModel _query;
        private List<SourceDto> _allSources = new();

        public ObservableCollection<SourceDto> Sources { get; } = new();

        [ObservableProperty]
        private SourceDto? selectedSource;

        [ObservableProperty]
        private bool isSourceCollectionVisible;

        public SourceViewModel(INewsService newsService, NewsQueryViewModel query)
        {
            _newsService = newsService;
            _query = query;
            _ = SafeLoadSources();
        }

        // Metod för att ladda alla källor
        public async Task LoadSourcesAsync()
        {
            try
            {
                _allSources = await _newsService.GetSourcesAsync();
                System.Diagnostics.Debug.WriteLine($"Loaded {_allSources.Count} sources total");

                // Filtrera baserat på nuvarande språk
                FilterSourcesByLanguage(_query.LanguageCode);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading sources: {ex.Message}");
            }
        }

        // Metod för att filtrera källor baserat på språk
        public void FilterSourcesByLanguage(string? languageCode)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Sources.Clear();

                Sources.Add(new SourceDto { Id = string.Empty, Name = "Alla källor" });

                if (string.IsNullOrEmpty(languageCode))
                {
                    foreach (var s in _allSources)
                        Sources.Add(s);
                }
                else
                {
                    var filteredSources = _allSources
                        .Where(s => s.Language?.Equals(languageCode, StringComparison.OrdinalIgnoreCase) == true)
                        .ToList();

                    foreach (var s in filteredSources)
                        Sources.Add(s);

                    System.Diagnostics.Debug.WriteLine($"Filtered to {filteredSources.Count} sources for language: {languageCode}");
                }
            });
        }

        // Partial metod som körs när SelectedSource ändras
        partial void OnSelectedSourceChanged(SourceDto? value)
        {
            IsSourceCollectionVisible = false;
            _query.SourceId = string.IsNullOrWhiteSpace(value?.Id) ? null : value.Id;

            System.Diagnostics.Debug.WriteLine($"Source changed to: {value?.Name ?? "All"}");
        }

        // Säker metod för att ladda källor med felhantering
        private async Task SafeLoadSources()
        {
            try
            {
                await LoadSourcesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }

}