using System;
using System.Collections.Generic;
using System.Text;

namespace NewNews.MAUI.Services
{
    public class BrowserService : IBrowserService
    {
        public Task OpenAsync(string url) =>
            Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
    }

}
