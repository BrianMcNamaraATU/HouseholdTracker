using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Core.ViewModels;

/// <summary>
/// ViewModel for the Forgot Password page
/// </summary>
public partial class ForgotPasswordViewModel : ObservableObject
{
    private readonly IApiService _apiService;

    /// <summary>
    /// Constructor for the ForgotPasswordViewModel
    /// </summary>
    /// <param name="apiService">The API service to use for password reset requests</param>
    public ForgotPasswordViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>The user's email address</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private string _email = string.Empty;

    /// <summary>Error message for the email field</summary>
    [ObservableProperty]
    private string _emailError = string.Empty;

    /// <summary>Whether an API call is currently in progress</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private bool _isBusy = false;

    /// <summary>Whether the request was submitted successfully</summary>
    [ObservableProperty]
    private bool _submitSuccessful = false;

    /// <summary>
    /// The success message shown after the request is submitted
    /// </summary>
    public string SuccessMessage => "If an account exists for this email address, a temporary password has been sent. Please check your inbox.";

    /// <summary>
    /// Validates the email field
    /// </summary>
    private bool ValidateFields()
    {
        EmailError = string.Empty;

        if (string.IsNullOrWhiteSpace(Email))
        {
            EmailError = "Email address is required.";
            return false;
        }
        if (Email.Length > 50)
        {
            EmailError = "Email address must be 50 characters or less.";
            return false;
        }
        if (!Email.Contains('@') || !Email.Contains('.'))
        {
            EmailError = "Please enter a valid email address.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Command to submit the forgot password request
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task SubmitAsync()
    {
        if (!ValidateFields()) return;

        IsBusy = true;
        try
        {
            await _apiService.ForgotPasswordAsync(Email.Trim().ToLower());

            // Always show success regardless of whether the email exists
            SubmitSuccessful = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Whether the submit command can execute
    /// </summary>
    private bool CanSubmit() => !IsBusy && !string.IsNullOrWhiteSpace(Email);
}
