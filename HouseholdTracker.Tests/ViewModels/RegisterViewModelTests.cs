using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;
using HouseholdTracker.Tests.Services;

namespace HouseholdTracker.Tests.ViewModels;

/// <summary>
/// Tests for the RegisterViewModel
/// </summary>
[TestFixture]
internal sealed class RegisterViewModelTests
{
    private TestApiService _apiService;
    private TestStorageLoggedInUserService _storage;
    private RegisterViewModel _viewModel;

    /// <summary>
    /// Create fresh instances before each test
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _apiService = new TestApiService();
        _storage = new TestStorageLoggedInUserService();
        LoggedInUserService.Initialize(_storage);
        _viewModel = new RegisterViewModel(_apiService, _storage);
    }

    /// <summary>
    /// Ensure an empty first name shows an error
    /// </summary>
    [Test]
    public async Task EmptyFirstName_ShowsFirstNameError()
    {
        SetValidFields();
        _viewModel.FirstName = string.Empty;
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.FirstNameError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a first name over 20 characters shows an error
    /// </summary>
    [Test]
    public async Task FirstNameTooLong_ShowsFirstNameError()
    {
        SetValidFields();
        _viewModel.FirstName = new string('a', 21);
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.FirstNameError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an empty last name shows an error
    /// </summary>
    [Test]
    public async Task EmptyLastName_ShowsLastNameError()
    {
        SetValidFields();
        _viewModel.LastName = string.Empty;
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.LastNameError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a last name over 20 characters shows an error
    /// </summary>
    [Test]
    public async Task LastNameTooLong_ShowsLastNameError()
    {
        SetValidFields();
        _viewModel.LastName = new string('a', 21);
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.LastNameError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an empty email shows an error
    /// </summary>
    [Test]
    public async Task EmptyEmail_ShowsEmailError()
    {
        SetValidFields();
        _viewModel.Email = string.Empty;
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an invalid email format shows an error
    /// </summary>
    [Test]
    public async Task InvalidEmail_ShowsEmailError()
    {
        SetValidFields();
        _viewModel.Email = "notanemail";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an email over 50 characters shows an error
    /// </summary>
    [Test]
    public async Task EmailTooLong_ShowsEmailError()
    {
        SetValidFields();
        _viewModel.Email = new string('a', 45) + "@test.com";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a password that is too short shows an error
    /// </summary>
    [Test]
    public async Task PasswordTooShort_ShowsPasswordError()
    {
        SetValidFields();
        _viewModel.Password = "Ab1!";
        _viewModel.ConfirmPassword = "Ab1!";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a password that is too long shows an error
    /// </summary>
    [Test]
    public async Task PasswordTooLong_ShowsPasswordError()
    {
        SetValidFields();
        _viewModel.Password = "Abcdefghij12345678!90";
        _viewModel.ConfirmPassword = "Abcdefghij12345678!90";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a password with no uppercase shows an error
    /// </summary>
    [Test]
    public async Task PasswordNoUppercase_ShowsPasswordError()
    {
        SetValidFields();
        _viewModel.Password = "password1!";
        _viewModel.ConfirmPassword = "password1!";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a password with no lowercase shows an error
    /// </summary>
    [Test]
    public async Task PasswordNoLowercase_ShowsPasswordError()
    {
        SetValidFields();
        _viewModel.Password = "PASSWORD1!";
        _viewModel.ConfirmPassword = "PASSWORD1!";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a password with no number shows an error
    /// </summary>
    [Test]
    public async Task PasswordNoNumber_ShowsPasswordError()
    {
        SetValidFields();
        _viewModel.Password = "Password!";
        _viewModel.ConfirmPassword = "Password!";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a password with no special character shows an error
    /// </summary>
    [Test]
    public async Task PasswordNoSpecialChar_ShowsPasswordError()
    {
        SetValidFields();
        _viewModel.Password = "Password1";
        _viewModel.ConfirmPassword = "Password1";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure mismatched passwords show a confirm password error
    /// </summary>
    [Test]
    public async Task PasswordMismatch_ShowsConfirmPasswordError()
    {
        SetValidFields();
        _viewModel.ConfirmPassword = "Different1!";
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.ConfirmPasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a duplicate email shows the correct error message with forgot password hint
    /// </summary>
    [Test]
    public async Task DuplicateEmail_ShowsEmailErrorWithForgotPasswordHint()
    {
        _apiService.RegisterResult = ApiResult<RegisteredUser>.Fail("A user with this email already exists.");
        SetValidFields();
        await _viewModel.RegisterCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Does.Contain("Forgot Password"));
    }

    /// <summary>
    /// Ensure a successful registration sets RegisterSuccessful to true
    /// </summary>
    [Test]
    public async Task SuccessfulRegistration_SetsRegisterSuccessful()
    {
        _apiService.RegisterResult = ApiResult<RegisteredUser>.Ok(
            new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1")
        );
        SetValidFields();
        await _viewModel.RegisterCommand.ExecuteAsync(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.RegisterSuccessful, Is.True);
            Assert.That(_viewModel.GeneralError, Is.Empty);
        }
    }

    /// <summary>
    /// Ensure a successful registration stores the user in SecureStorage
    /// </summary>
    [Test]
    public async Task SuccessfulRegistration_StoresUserInStorage()
    {
        _apiService.RegisterResult = ApiResult<RegisteredUser>.Ok(
            new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1")
        );
        SetValidFields();
        await _viewModel.RegisterCommand.ExecuteAsync(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await LoggedInUserService.GetLoggedInUserIdAsync(), Is.EqualTo(1));
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.EqualTo("apikey-1"));
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.EqualTo("Brian"));
        }
    }

    /// <summary>
    /// Sets all fields to valid values for use in tests that only need to test one field
    /// </summary>
    private void SetValidFields()
    {
        _viewModel.FirstName = "Brian";
        _viewModel.LastName = "Smith";
        _viewModel.Email = "brian@example.com";
        _viewModel.Password = "Password1!";
        _viewModel.ConfirmPassword = "Password1!";
    }
}
