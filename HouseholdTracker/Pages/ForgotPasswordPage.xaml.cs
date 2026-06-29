using HouseholdTracker.Core.ViewModels;

namespace HouseholdTracker.Pages;

/// <summary>
/// The Forgot Password page — allows a user to request a temporary password
/// </summary>
public partial class ForgotPasswordPage : ContentPage
{
    /// <summary>
    /// Constructor for the ForgotPasswordPage
    /// </summary>
    /// <param name="viewModel">The ForgotPasswordViewModel</param>
    public ForgotPasswordPage(ForgotPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    /// <summary>
    /// Navigates back to the Login page
    /// </summary>
    private async void OnBackToLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
