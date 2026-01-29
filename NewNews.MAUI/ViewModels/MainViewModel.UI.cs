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
        private void ToggleNewsExpanded(ArticleViewModel article)
        {
            if (article == null) return;

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

        private double webViewHeight;
        public double WebViewHeight
        {
            get => webViewHeight;
            set => SetProperty(ref webViewHeight, value);
        }

        //[RelayCommand]
        //private async Task WebViewNavigated((News article, WebView webView) param)
        //{
        //    var (article, webView) = param;
        //    if (article == null || webView == null) return;

        //    try
        //    {
        //        var heightString = await webView.EvaluateJavaScriptAsync("document.body.scrollHeight.toString()");
        //        if (double.TryParse(heightString, out double height))
        //        {
        //            article.WebViewHeight = height;
        //        }
        //    }
        //    catch { }
        //}




    }
}
