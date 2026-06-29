using HouseholdTracker.Core.Services;

namespace HouseholdTracker;

/// <summary>
/// The main application class
/// </summary>
public partial class App : Application
{
    private readonly AppShell _appShell;

    /// <summary>
    /// Constructor for the App
    /// </summary>
    /// <param name="appShell">The AppShell</param>
    public App(AppShell appShell)
    {
        InitializeComponent();
        _appShell = appShell;
    }

    /// <inheritdoc />
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_appShell);
    }

    protected override async void OnStart()
    {
        base.OnStart();
        await CheckLoginStateAsync();
    }

    private static async Task CheckLoginStateAsync()
    {
        var userId = await LoggedInUserService.GetLoggedInUserIdAsync();
        if (userId is not null)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            });
            await Shell.Current.GoToAsync("//MyTrackingPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
