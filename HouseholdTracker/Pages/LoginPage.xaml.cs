using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;

namespace HouseholdTracker.Pages;

/// <summary>
/// The Login page — the initial landing page when no user session exists
/// </summary>
public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    /// <summary>
    /// Constructor for the LoginPage
    /// </summary>
    /// <param name="viewModel">The LoginViewModel</param>
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // Wire up navigation callback directly
        _viewModel.OnLoginSuccess = async (forcePasswordReset) =>
        {
            if (forcePasswordReset)
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
                NavigationState.ForcePasswordChange = true;
                await Shell.Current.GoToAsync("//ChangePasswordPage");
            }
            else
            {
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                await Shell.Current.GoToAsync("//MyTrackingPage");
            }
        };
    }

    /// <inheritdoc />
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    /// <inheritdoc />
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

    /// <summary>
    /// Navigates to the Forgot Password page
    /// </summary>
    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ForgotPasswordPage");
    }

    /// <summary>
    /// Navigates to the Register page
    /// </summary>
    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");
    }
}
