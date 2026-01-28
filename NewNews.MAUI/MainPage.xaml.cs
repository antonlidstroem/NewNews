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

        private void SavedKeywords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is SavedSearch selected)
            {
                // Anropa samma funktion som när man väljer i dropdown
                var vm = BindingContext as MainViewModel;
                vm?.SearchByKeywordCommand.Execute(selected);

                // Avmarkera direkt så man kan klicka igen
                ((CollectionView)sender).SelectedItem = null;

                // Fäll ihop listan efter val
                if (vm != null)
                    vm.AreSavedKeywordsVisible = false;
            }
        }



    }
}
