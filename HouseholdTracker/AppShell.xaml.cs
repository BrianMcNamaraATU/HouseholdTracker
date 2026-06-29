using HouseholdTracker.Core.Services;

namespace HouseholdTracker;

/// <summary>
/// The AppShell — manages navigation and the flyout menu
/// </summary>
public partial class AppShell : Shell
{
    private readonly IApiService _apiService;

    /// <summary>
    /// Constructor for the AppShell
    /// </summary>
    /// <param name="apiService">The API service</param>
    public AppShell(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;

        Items.Add(CreateLogoutMenuItem());
    }

    /// <summary>
    /// Creates the logout menu item for the flyout
    /// </summary>
    private MenuItem CreateLogoutMenuItem()
    {
        var logout = new MenuItem
        {
            Text = "Logout"
        };
        logout.Clicked += OnLogoutClicked;
        return logout;
    }

    /// <summary>
    /// Handles the logout menu item click — clears storage and returns to Login
    /// </summary>
    private async void OnLogoutClicked(object? sender, EventArgs e)
    {
        await LoggedInUserService.LogoutUserAsync();
        await GoToAsync("//LoginPage");
    }
}
