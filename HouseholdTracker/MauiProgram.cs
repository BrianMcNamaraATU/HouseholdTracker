using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;
using HouseholdTracker.Pages;
using HouseholdTracker.Services;
using Microsoft.Extensions.Logging;

namespace HouseholdTracker;

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

        // Database location
        var localDbPath = Path.Combine(FileSystem.AppDataDirectory, "localDatabase.db");

        // Storage
        var storageService = new MauiStorageLoggedInUserService();
        LoggedInUserService.Initialize(storageService);
        builder.Services.AddSingleton<IStorageLoggedInUserService>(storageService);

        // HTTP Client and API Service
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton<IApiService, ApiService>();

        // Local database
        builder.Services.AddSingleton(new LocalDatabaseService(localDbPath));

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<ForgotPasswordViewModel>();
        builder.Services.AddTransient<ChangePasswordViewModel>();

        // Pages
        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<ForgotPasswordPage>();
        builder.Services.AddTransient<ChangePasswordPage>();
        builder.Services.AddTransient<MyTrackingPage>();

        // Shell
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}
