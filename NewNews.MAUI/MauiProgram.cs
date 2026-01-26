using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewNews.DAL.Services;
using NewNews.MAUI.ViewModels;


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

            // SQLite-databas
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "searchkeywords.db3");
            builder.Services.AddSingleton(new SearchKeywordService(dbPath));

            // Registrera NewsService med HttpClient
            builder.Services.AddHttpClient<NewsService>(client =>
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NewNewsApp/1.0");
            });

            // Registrera MainViewModel som singleton
            builder.Services.AddSingleton<MainViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
