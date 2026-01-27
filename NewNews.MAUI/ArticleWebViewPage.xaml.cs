using Microsoft.Maui.Controls;

namespace NewNews.MAUI;

public partial class ArticleWebViewPage : ContentPage
{
	public ArticleWebViewPage(string url)
	{
		InitializeComponent();
		WebViewControl.Source = url;
    }
}