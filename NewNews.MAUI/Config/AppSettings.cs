using System;
using System.Collections.Generic;
using System.Text;

namespace NewNews.MAUI.Configuration
{
    public class AppSettings
    {
        public NewsApiSettings NewsApi { get; set; } = new();
    }

    public class NewsApiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
