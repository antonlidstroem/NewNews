using NewNews.DAL.Models;
using NewNews.MAUI.ViewModels;


namespace NewNews.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;        
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MainViewModel vm)
            {
                //await vm.LoadMoreNews();
            }
        }

        private async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (sender is WebView webView && webView.BindingContext is News article)
            {
                try
                {
                    var heightStr = await webView.EvaluateJavaScriptAsync("document.body.scrollHeight.toString()");
                    if (double.TryParse(heightStr, out double height))
                    {
                        article.WebViewHeight = height;
                    }
                }
                catch { }
            }
        }

    }
}
