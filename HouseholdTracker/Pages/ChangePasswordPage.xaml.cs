using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;

namespace HouseholdTracker.Pages;

/// <summary>
/// The Change Password page — allows a user to change their password.
/// When accessed due to a forced reset, the user cannot navigate away until complete.
/// </summary>
[QueryProperty(nameof(IsForced), "IsForced")]
public partial class ChangePasswordPage : ContentPage
{
    private readonly ChangePasswordViewModel _viewModel;

    /// <summary>
    /// Constructor for the ChangePasswordPage
    /// </summary>
    /// <param name="viewModel">The ChangePasswordViewModel</param>
    public ChangePasswordPage(ChangePasswordViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Whether the password change is forced — set via Shell navigation parameter
    /// </summary>
    public bool IsForced
    {
        set => _viewModel.IsForced = value;
    }

    /// <inheritdoc />
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.IsForced = NavigationState.ForcePasswordChange;
        NavigationState.ForcePasswordChange = false;

        // Only disable the flyout if this is a forced password reset
        Shell.Current.FlyoutBehavior = _viewModel.IsForced
            ? FlyoutBehavior.Disabled
            : FlyoutBehavior.Flyout;

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    /// <inheritdoc />
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }

    /// <summary>
    /// Handles property changes on the ViewModel to trigger navigation
    /// </summary>
    private async void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ChangePasswordViewModel.ChangeSuccessful) && _viewModel.ChangeSuccessful)
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync("//MyTrackingPage");
        }
    }

    /// <summary>
    /// Prevents back navigation when the password change is forced
    /// </summary>
    protected override bool OnBackButtonPressed()
    {
        if (_viewModel.IsForced)
            return true;
        return base.OnBackButtonPressed();
    }
}
