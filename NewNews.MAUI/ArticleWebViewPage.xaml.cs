using Microsoft.Maui.Controls;

namespace NewNews.MAUI;

[QueryProperty(nameof(Url), "url")]
public partial class ArticleWebViewPage : ContentPage
{
    public string Url
    {
        get => WebViewControl.Source?.ToString() ?? string.Empty;
        set => WebViewControl.Source = value;
    }
    public ArticleWebViewPage()
	{
		InitializeComponent();
		//WebViewControl.Source = url;
    }
}