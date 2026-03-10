using HouseholdTracker.Core.Services;
using Microsoft.Extensions.Logging;

namespace HouseholdTracker
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

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Database locations
            string centralDbPath = Path.Combine(FileSystem.AppDataDirectory, "centralDatabase.db");
            string localDbPath = Path.Combine(FileSystem.AppDataDirectory, "localDatabase.db");

            // Add a reference to the Services
            builder.Services.AddSingleton(new CentralDatabaseService(centralDbPath));


            return builder.Build();
        }
    }
}
