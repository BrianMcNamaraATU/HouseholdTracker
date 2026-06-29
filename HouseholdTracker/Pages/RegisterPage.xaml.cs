using HouseholdTracker.Core.ViewModels;

namespace HouseholdTracker.Pages;

/// <summary>
/// The Register page — allows a new user to create an account
/// </summary>
public partial class RegisterPage : ContentPage
{
    private readonly RegisterViewModel _viewModel;

    /// <summary>
    /// Constructor for the RegisterPage
    /// </summary>
    /// <param name="viewModel">The RegisterViewModel</param>
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// <inheritdoc />
    protected override void OnAppearing()
    {
        base.OnAppearing();
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
        if (e.PropertyName == nameof(RegisterViewModel.RegisterSuccessful) && _viewModel.RegisterSuccessful)
        {
            await Shell.Current.GoToAsync("//MyTrackingPage");
        }
    }

    /// <summary>
    /// Navigates back to the Login page
    /// </summary>
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
