using Microsoft.Extensions.Logging;
using ProfileAss.Service;
using ProfileAss.ViewModel;
using ProfileAss.Views;
using ProfileAss.Data;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Maui;
//using .Services;
using ProfileAss.Service;

namespace ProfileAss
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "shoesstores.db");
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
            builder.Services.AddSingleton<BadgeCounterService>();
            // Register ViewModels
            builder.Services.AddSingleton<ProfileViewModel>();
            
            builder.Services.AddTransient<BasketViewModel>();
            builder.Services.AddTransient<ProductViewModel>();

            // Register Pages
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddSingleton<BasketPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
