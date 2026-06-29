using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Core.ViewModels;

/// <summary>
/// ViewModel for the Login page
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    private readonly IApiService _apiService;
    private readonly IStorageLoggedInUserService _storage;

    /// <summary>
    /// Action invoked on successful login — passes true if password reset required
    /// </summary>
    public Action<bool>? OnLoginSuccess { get; set; }

    /// <summary>
    /// Constructor for the LoginViewModel
    /// </summary>
    /// <param name="apiService">The API service to use for login requests</param>
    /// <param name="storage">The storage service for persisting the logged in user</param>
    public LoginViewModel(IApiService apiService, IStorageLoggedInUserService storage)
    {
        _apiService = apiService;
        _storage = storage;
    }

    /// <summary>The user's email address</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _email = string.Empty;

    /// <summary>The user's password</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _password = string.Empty;

    /// <summary>Error message for the email field</summary>
    [ObservableProperty]
    private string _emailError = string.Empty;

    /// <summary>Error message for the password field</summary>
    [ObservableProperty]
    private string _passwordError = string.Empty;

    /// <summary>General error message for failed login attempts</summary>
    [ObservableProperty]
    private string _generalError = string.Empty;

    /// <summary>Whether an API call is currently in progress</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool _isBusy = false;

    /// <summary>Whether the login was successful and requires a password reset</summary>
    [ObservableProperty]
    private bool _forcePasswordReset = false;

    /// <summary>Whether the login was successful</summary>
    [ObservableProperty]
    private bool _loginSuccessful = false;

    /// <summary>
    /// Validates all fields and returns true if they are all valid
    /// </summary>
    private bool ValidateFields()
    {
        var valid = true;

        EmailError = string.Empty;
        PasswordError = string.Empty;
        GeneralError = string.Empty;

        if (string.IsNullOrWhiteSpace(Email))
        {
            EmailError = "Email address is required.";
            valid = false;
        }
        else if (Email.Length > 50)
        {
            EmailError = "Email address must be 50 characters or less.";
            valid = false;
        }
        else if (!Email.Contains('@') || !Email.Contains('.'))
        {
            EmailError = "Please enter a valid email address.";
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            PasswordError = "Password is required.";
            valid = false;
        }

        return valid;
    }

    /// <summary>
    /// Command to login the user
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        if (!ValidateFields()) return;

        IsBusy = true;
        try
        {
            var result = await _apiService.LoginAsync(Email.Trim().ToLower(), Password);

            if (!result.Success || result.Data is null)
            {
                GeneralError = result.ErrorMessage ?? "Login failed. Please try again.";
                return;
            }

            await LoggedInUserService.LoginUserAsync(result.Data);

            // Invoke navigation action directly with the force password reset flag
            OnLoginSuccess?.Invoke(result.Data.ForcePasswordReset);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanLogin() => !IsBusy && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
}
