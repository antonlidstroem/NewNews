//using NewNews.MAUI.ViewModels;
//using Microsoft.Extensions.Logging;
//using NewNews.DAL.Services;

//namespace NewNews.MAUI
//{
//    public static class MauiProgram
//    {
//        public static MauiApp CreateMauiApp()
//        {
//            var builder = MauiApp.CreateBuilder();
//            builder
//                .UseMauiApp<App>()
//                .ConfigureFonts(fonts =>
//                {
//                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
//                });

//            builder.Services.AddHttpClient<NewsService>(client =>
//            {
//                client.DefaultRequestHeaders.UserAgent.ParseAdd("NewNewsApp/1.0");
//            });

//            // Registrera MainViewModel som singleton
//            builder.Services.AddSingleton<MainViewModel>();

//            builder.Services.AddSingleton<NewsService>();
//            builder.Services.AddSingleton<MainViewModel>();



//#if DEBUG
//            builder.Logging.AddDebug();
//#endif

//            return builder.Build();
//        }
//    }
//}


using NewNews.MAUI.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


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
