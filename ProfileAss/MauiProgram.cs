using Microsoft.Extensions.Logging;
using ProfileAss.Service;
using ProfileAss.ViewModel;
using ProfileAss.Views;
using ProfileAss.Data;
using Microsoft.EntityFrameworkCore;

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

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "profile.db");
            builder.Services.AddDbContext<DatabaseContext>( options =>
            {
                options.UseSqlite($"Data Source={dbPath}");
            });

            // Force database creation on startup
            var serviceProvider = builder.Services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.EnsureCreated();
            }

            // Register database context
            builder.Services.AddDbContext<DatabaseContext>();

            // Register services
            builder.Services.AddScoped<IDataService, DataService>();

            // Register ViewModels
            builder.Services.AddTransient<ProfileViewModel>();

            // Register Pages
            builder.Services.AddTransient<ProfilePage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
