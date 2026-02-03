namespace NewNews.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ArticleWebViewPage), typeof(ArticleWebViewPage));
        }
    }
}
