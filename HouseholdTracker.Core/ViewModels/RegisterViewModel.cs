using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Core.ViewModels;

/// <summary>
/// ViewModel for the Register page
/// </summary>
public partial class RegisterViewModel : ObservableObject
{
    private readonly IApiService _apiService;
    private readonly IStorageLoggedInUserService _storage;

    /// <summary>
    /// Constructor for the RegisterViewModel
    /// </summary>
    /// <param name="apiService">The API service to use for registration requests</param>
    /// <param name="storage">The storage service for persisting the logged in user</param>
    public RegisterViewModel(IApiService apiService, IStorageLoggedInUserService storage)
    {
        _apiService = apiService;
        _storage = storage;
    }

    /// <summary>The user's first name</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _firstName = string.Empty;

    /// <summary>The user's last name</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _lastName = string.Empty;

    /// <summary>The user's email address</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _email = string.Empty;

    /// <summary>The user's chosen password</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _password = string.Empty;

    /// <summary>The user's confirmed password</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _confirmPassword = string.Empty;

    /// <summary>Error message for the first name field</summary>
    [ObservableProperty]
    private string _firstNameError = string.Empty;

    /// <summary>Error message for the last name field</summary>
    [ObservableProperty]
    private string _lastNameError = string.Empty;

    /// <summary>Error message for the email field</summary>
    [ObservableProperty]
    private string _emailError = string.Empty;

    /// <summary>Error message for the password field</summary>
    [ObservableProperty]
    private string _passwordError = string.Empty;

    /// <summary>Error message for the confirm password field</summary>
    [ObservableProperty]
    private string _confirmPasswordError = string.Empty;

    /// <summary>General error message for failed registration attempts</summary>
    [ObservableProperty]
    private string _generalError = string.Empty;

    /// <summary>Whether an API call is currently in progress</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private bool _isBusy = false;

    /// <summary>Whether the registration was successful</summary>
    [ObservableProperty]
    private bool _registerSuccessful = false;

    /// <summary>
    /// Validates all fields and returns true if they are all valid
    /// </summary>
    private bool ValidateFields()
    {
        var valid = true;

        FirstNameError = string.Empty;
        LastNameError = string.Empty;
        EmailError = string.Empty;
        PasswordError = string.Empty;
        ConfirmPasswordError = string.Empty;
        GeneralError = string.Empty;

        if (string.IsNullOrWhiteSpace(FirstName))
        {
            FirstNameError = "First name is required.";
            valid = false;
        }
        else if (FirstName.Length > 20)
        {
            FirstNameError = "First name must be 20 characters or less.";
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            LastNameError = "Last name is required.";
            valid = false;
        }
        else if (LastName.Length > 20)
        {
            LastNameError = "Last name must be 20 characters or less.";
            valid = false;
        }

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
        else if (!ValidatePassword(Password, out var passwordMessage))
        {
            PasswordError = passwordMessage;
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ConfirmPasswordError = "Please confirm your password.";
            valid = false;
        }
        else if (Password != ConfirmPassword)
        {
            ConfirmPasswordError = "Passwords do not match.";
            valid = false;
        }

        return valid;
    }

    /// <summary>
    /// Validates the password against the complexity rules
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <param name="message">The error message if validation fails</param>
    /// <returns>True if the password is valid</returns>
    internal static bool ValidatePassword(string password, out string message)
    {
        message = string.Empty;

        if (password.Length < 5 || password.Length > 20)
        {
            message = "Password must be between 5 and 20 characters.";
            return false;
        }
        if (!password.Any(char.IsUpper))
        {
            message = "Password must contain at least one uppercase letter.";
            return false;
        }
        if (!password.Any(char.IsLower))
        {
            message = "Password must contain at least one lowercase letter.";
            return false;
        }
        if (!password.Any(char.IsDigit))
        {
            message = "Password must contain at least one number.";
            return false;
        }
        if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            message = "Password must contain at least one special character.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Command to register a new user
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task RegisterAsync()
    {
        if (!ValidateFields()) return;

        IsBusy = true;
        try
        {
            var result = await _apiService.RegisterAsync(
                FirstName.Trim(),
                LastName.Trim(),
                Email.Trim().ToLower(),
                Password
            );

            if (!result.Success || result.Data is null)
            {
                // Check for duplicate email specifically
                if (result.ErrorMessage?.Contains("already") == true)
                    EmailError = "This email is already registered. If you have forgotten your password, please use the Forgot Password link.";
                else
                    GeneralError = result.ErrorMessage ?? "Registration failed. Please try again.";
                return;
            }

            await LoggedInUserService.LoginUserAsync(result.Data);
            RegisterSuccessful = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Whether the register command can execute
    /// </summary>
    private bool CanRegister() =>
        !IsBusy &&
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password) &&
        !string.IsNullOrWhiteSpace(ConfirmPassword);
}
