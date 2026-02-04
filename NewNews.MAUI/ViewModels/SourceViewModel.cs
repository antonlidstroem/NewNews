using System.Collections.ObjectModel;
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

        public ObservableCollection<SourceDto> Sources { get; } = new();

        [ObservableProperty]
        private SourceDto? selectedSource;

        [ObservableProperty]
        private bool isSourceCollectionVisible;

        public SourceViewModel(INewsService newsService, NewsQueryViewModel query)
        {
            _newsService = newsService;
            _query = query;
            _ = LoadSourcesAsync();
        }

        public async Task LoadSourcesAsync()
        {
            try
            {
                Sources.Clear();
                var sources = await _newsService.GetSourcesAsync();

                Sources.Add(new SourceDto { Id = string.Empty, Name = "Alla källor" });

                foreach (var s in sources)
                    Sources.Add(s);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading sources: {ex.Message}");
            }
        }

        partial void OnSelectedSourceChanged(SourceDto? value)
        {
            IsSourceCollectionVisible = false;
            _query.SourceId = string.IsNullOrWhiteSpace(value?.Id) ? null : value.Id;
        }
    }
}