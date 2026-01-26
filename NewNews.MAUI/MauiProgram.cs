using NewNews.MAUI.ViewModels;
using Microsoft.Extensions.Logging;
using NewNews.DAL.Services;

namespace NewNews.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(sp =>
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NewNewsApp/1.0");
                return client;
            });
            builder.Services.AddSingleton<NewsService>();
            builder.Services.AddSingleton<MainViewModel>();
            


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
