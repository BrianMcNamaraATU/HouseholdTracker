using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;
using HouseholdTracker.Tests.Services;

namespace HouseholdTracker.Tests.ViewModels;

/// <summary>
/// Tests for the ChangePasswordViewModel
/// </summary>
[TestFixture]
internal sealed class ChangePasswordViewModelTests
{
    private TestApiService _apiService;
    private TestStorageLoggedInUserService _storage;
    private ChangePasswordViewModel _viewModel;

    /// <summary>
    /// Create fresh instances before each test and log in a test user
    /// </summary>
    [SetUp]
    public async Task SetUp()
    {
        _apiService = new TestApiService();
        _storage = new TestStorageLoggedInUserService();
        LoggedInUserService.Initialize(_storage);

        // Log in a test user so the ViewModel can retrieve their details
        await LoggedInUserService.LoginUserAsync(
            new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1")
        );

        _viewModel = new ChangePasswordViewModel(_apiService);
    }

    /// <summary>
    /// Ensure an empty current password shows an error
    /// </summary>
    [Test]
    public async Task EmptyCurrentPassword_ShowsCurrentPasswordError()
    {
        SetValidFields();
        _viewModel.CurrentPassword = string.Empty;
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
        Assert.That(_viewModel.CurrentPasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an empty new password shows an error
    /// </summary>
    [Test]
    public async Task EmptyNewPassword_ShowsNewPasswordError()
    {
        SetValidFields();
        _viewModel.NewPassword = string.Empty;
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
        Assert.That(_viewModel.NewPasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a new password that is too short shows an error
    /// </summary>
    [Test]
    public async Task NewPasswordTooShort_ShowsNewPasswordError()
    {
        SetValidFields();
        _viewModel.NewPassword = "Ab1!";
        _viewModel.ConfirmNewPassword = "Ab1!";
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
        Assert.That(_viewModel.NewPasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a new password with no uppercase shows an error
    /// </summary>
    [Test]
    public async Task NewPasswordNoUppercase_ShowsNewPasswordError()
    {
        SetValidFields();
        _viewModel.NewPassword = "newpassword1!";
        _viewModel.ConfirmNewPassword = "newpassword1!";
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
        Assert.That(_viewModel.NewPasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure mismatched new passwords show a confirm password error
    /// </summary>
    [Test]
    public async Task PasswordMismatch_ShowsConfirmPasswordError()
    {
        SetValidFields();
        _viewModel.ConfirmNewPassword = "Different1!";
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
        Assert.That(_viewModel.ConfirmNewPasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a failed API response shows a general error
    /// </summary>
    [Test]
    public async Task FailedPasswordChange_ShowsGeneralError()
    {
        _apiService.ChangePasswordResult = ApiResult<bool>.Fail("Current password is incorrect.");
        SetValidFields();
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);
        Assert.That(_viewModel.GeneralError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a successful password change sets ChangeSuccessful to true
    /// </summary>
    [Test]
    public async Task SuccessfulPasswordChange_SetsChangeSuccessful()
    {
        _apiService.ChangePasswordResult = ApiResult<bool>.Ok(true);
        SetValidFields();
        await _viewModel.ChangePasswordCommand.ExecuteAsync(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.ChangeSuccessful, Is.True);
            Assert.That(_viewModel.GeneralError, Is.Empty);
        }
    }

    /// <summary>
    /// Sets all fields to valid values for use in tests that only need to test one field
    /// </summary>
    private void SetValidFields()
    {
        _viewModel.CurrentPassword = "OldPassword1!";
        _viewModel.NewPassword = "NewPassword1!";
        _viewModel.ConfirmNewPassword = "NewPassword1!";
    }
}
