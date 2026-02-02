using Microsoft.Maui.Controls;

namespace NewNews.MAUI;

[QueryProperty(nameof(Url), "url")]
public partial class ArticleWebViewPage : ContentPage
{
    private string _url = string.Empty;

    public string Url
    {
        get => _url;
        set
        {
            _url = Uri.UnescapeDataString(value ?? "");
            WebViewControl.Source = _url;
        }
    }

    public ArticleWebViewPage()
	{
		InitializeComponent();
		//WebViewControl.Source = url;
    }
}