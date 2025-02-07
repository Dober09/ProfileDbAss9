using Microsoft.Extensions.Logging;
using ProfileAss.Service;
using ProfileAss.ViewModel;
using ProfileAss.Views;

namespace ProfileAss
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

            builder.Services.AddSingleton<DataService>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<ProfilePage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
