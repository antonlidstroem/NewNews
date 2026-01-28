using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using NewNews.DAL.Models;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel
    {


        [RelayCommand]
        private void ToggleNewsExpanded(News article)
        {
            if (article == null) return;

            // Om du vill att bara en artikel kan vara öppen åt gången:
            foreach (var a in Articles)
                if (a != article)
                    a.IsExpanded = false;

            article.IsExpanded = !article.IsExpanded;
        }

        [RelayCommand]
        private async Task SourceTapped(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                // Navigera till WebView-sidan
                if (Application.Current.MainPage is not null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new ArticleWebViewPage(url));
                }
            }
        }

        [RelayCommand]
        private async Task OpenInBrowser(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }

    }
}
