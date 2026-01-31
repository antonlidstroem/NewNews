using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NewNews.DAL.Services;
using NewNews.MAUI.Services;
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

            // Lägg till appsettings.json
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            // SQLite-databas
            builder.Services.AddSingleton<SearchKeywordService>(sp =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "searchkeywords.db3");
                return new SearchKeywordService(dbPath);
            });




            // Registrera NewsApiClient med HttpClient
            builder.Services.AddHttpClient<INewsApiClient, NewsApiClient>(client =>
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NewNewsApp/1.0");
            });

            // Registrera Services
            builder.Services.AddSingleton<IBrowserService, BrowserService>();
            builder.Services.AddSingleton<INewsService, NewsService>();

            // Registrera MainViewModel som singleton
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<MainPage>();


            #if DEBUG
            builder.Logging.AddDebug();
            #endif

            return builder.Build();
        }
    }

}

