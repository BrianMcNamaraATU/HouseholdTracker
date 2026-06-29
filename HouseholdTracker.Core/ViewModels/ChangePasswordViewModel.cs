using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Core.ViewModels;

/// <summary>
/// ViewModel for the Change Password page
/// </summary>
public partial class ChangePasswordViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    /// <summary>
    /// Constructor for the ChangePasswordViewModel
    /// </summary>
    /// <param name="apiService">The API service to use for password change requests</param>
    public ChangePasswordViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>The user's current password</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
    private string _currentPassword = string.Empty;

    /// <summary>The user's new password</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
    private string _newPassword = string.Empty;

    /// <summary>The user's confirmed new password</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
    private string _confirmNewPassword = string.Empty;

    /// <summary>Error message for the current password field</summary>
    [ObservableProperty]
    private string _currentPasswordError = string.Empty;

    /// <summary>Error message for the new password field</summary>
    [ObservableProperty]
    private string _newPasswordError = string.Empty;

    /// <summary>Error message for the confirm new password field</summary>
    [ObservableProperty]
    private string _confirmNewPasswordError = string.Empty;

    /// <summary>General error message for failed password change attempts</summary>
    [ObservableProperty]
    private string _generalError = string.Empty;

    /// <summary>Whether an API call is currently in progress</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
    private bool _isBusy = false;

    /// <summary>Whether the password change was successful</summary>
    [ObservableProperty]
    private bool _changeSuccessful = false;

    /// <summary>
    /// Whether the password change is forced due to a password reset request.
    /// When true, the user cannot navigate away until the password is changed.
    /// </summary>
    [ObservableProperty]
    private bool _isForced = false;

    /// <summary>
    /// Validates all fields and returns true if they are all valid
    /// </summary>
    private bool ValidateFields()
    {
        var valid = true;

        CurrentPasswordError = string.Empty;
        NewPasswordError = string.Empty;
        ConfirmNewPasswordError = string.Empty;
        GeneralError = string.Empty;

        if (string.IsNullOrWhiteSpace(CurrentPassword))
        {
            CurrentPasswordError = "Current password is required.";
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(NewPassword))
        {
            NewPasswordError = "New password is required.";
            valid = false;
        }
        else if (!RegisterViewModel.ValidatePassword(NewPassword, out var passwordMessage))
        {
            NewPasswordError = passwordMessage;
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(ConfirmNewPassword))
        {
            ConfirmNewPasswordError = "Please confirm your new password.";
            valid = false;
        }
        else if (NewPassword != ConfirmNewPassword)
        {
            ConfirmNewPasswordError = "Passwords do not match.";
            valid = false;
        }

        return valid;
    }

    /// <summary>
    /// Command to change the user's password
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanChangePassword))]
    private async Task ChangePasswordAsync()
    {
        if (!ValidateFields()) return;

        IsBusy = true;
        try
        {
            var userId = await LoggedInUserService.GetLoggedInUserIdAsync();
            var apiKey = await LoggedInUserService.GetLoggedInUserAPIKeyAsync();
            var email = await LoggedInUserService.GetLoggedInUserEmailAsync();
            System.Diagnostics.Debug.WriteLine($"ChangePassword: userId={userId}, apiKey={apiKey?.Substring(0, 8)}, email={email}");

            if (userId is null || apiKey is null || email is null)
            {
                GeneralError = "Session expired. Please log in again.";
                return;
            }

            var result = await _apiService.ChangePasswordAsync(
                userId.Value,
                apiKey,
                email,
                CurrentPassword,
                NewPassword
            );

            if (!result.Success)
            {
                GeneralError = result.ErrorMessage ?? "Password change failed. Please try again.";
                return;
            }

            ChangeSuccessful = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Whether the change password command can execute
    /// </summary>
    private bool CanChangePassword() =>
        !IsBusy &&
        !string.IsNullOrWhiteSpace(CurrentPassword) &&
        !string.IsNullOrWhiteSpace(NewPassword) &&
        !string.IsNullOrWhiteSpace(ConfirmNewPassword);
}
