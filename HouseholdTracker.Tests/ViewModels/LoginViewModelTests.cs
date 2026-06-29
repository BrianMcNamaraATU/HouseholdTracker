using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;
using HouseholdTracker.Tests.Services;

namespace HouseholdTracker.Tests.ViewModels;

/// <summary>
/// Tests for the LoginViewModel
/// </summary>
[TestFixture]
internal sealed class LoginViewModelTests
{
    private TestApiService _apiService;
    private TestStorageLoggedInUserService _storage;
    private LoginViewModel _viewModel;

    /// <summary>
    /// Create fresh instances before each test
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _apiService = new TestApiService();
        _storage = new TestStorageLoggedInUserService();
        LoggedInUserService.Initialize(_storage);
        _viewModel = new LoginViewModel(_apiService, _storage);
    }

    /// <summary>
    /// Ensure an empty email shows an error
    /// </summary>
    [Test]
    public async Task EmptyEmail_ShowsEmailError()
    {
        _viewModel.Email = string.Empty;
        _viewModel.Password = "Password1!";
        await _viewModel.LoginCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an invalid email format shows an error
    /// </summary>
    [Test]
    public async Task InvalidEmailFormat_ShowsEmailError()
    {
        _viewModel.Email = "notanemail";
        _viewModel.Password = "Password1!";
        await _viewModel.LoginCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an email over 50 characters shows an error
    /// </summary>
    [Test]
    public async Task EmailTooLong_ShowsEmailError()
    {
        _viewModel.Email = new string('a', 45) + "@test.com";
        _viewModel.Password = "Password1!";
        await _viewModel.LoginCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an empty password shows an error
    /// </summary>
    [Test]
    public async Task EmptyPassword_ShowsPasswordError()
    {
        _viewModel.Email = "brian@example.com";
        _viewModel.Password = string.Empty;
        await _viewModel.LoginCommand.ExecuteAsync(null);
        Assert.That(_viewModel.PasswordError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a failed API response shows a general error
    /// </summary>
    [Test]
    public async Task FailedLogin_ShowsGeneralError()
    {
        _apiService.LoginResult = ApiResult<RegisteredUser>.Fail("Invalid email or password.");
        _viewModel.Email = "brian@example.com";
        _viewModel.Password = "Password1!";
        await _viewModel.LoginCommand.ExecuteAsync(null);
        Assert.That(_viewModel.GeneralError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a successful login sets LoginSuccessful to true
    /// </summary>
    [Test]
    public async Task SuccessfulLogin_SetsLoginSuccessful()
    {
        _apiService.LoginResult = ApiResult<RegisteredUser>.Ok(
            new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1")
        );
        _viewModel.Email = "brian@example.com";
        _viewModel.Password = "Password1!";
        await _viewModel.LoginCommand.ExecuteAsync(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.LoginSuccessful, Is.True);
            Assert.That(_viewModel.GeneralError, Is.Empty);
        }
    }

    /// <summary>
    /// Ensure a successful login with force password reset sets ForcePasswordReset
    /// </summary>
    [Test]
    public async Task SuccessfulLogin_WithForcePasswordReset_SetsFlag()
    {
        _apiService.LoginResult = ApiResult<RegisteredUser>.Ok(
            new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1", forcePasswordReset: true)
        );
        _viewModel.Email = "brian@example.com";
        _viewModel.Password = "TempPass123";
        await _viewModel.LoginCommand.ExecuteAsync(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.LoginSuccessful, Is.True);
            Assert.That(_viewModel.ForcePasswordReset, Is.True);
        }
    }

    /// <summary>
    /// Ensure a successful login stores the user in SecureStorage
    /// </summary>
    [Test]
    public async Task SuccessfulLogin_StoresUserInStorage()
    {
        _apiService.LoginResult = ApiResult<RegisteredUser>.Ok(
            new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1")
        );
        _viewModel.Email = "brian@example.com";
        _viewModel.Password = "Password1!";
        await _viewModel.LoginCommand.ExecuteAsync(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await LoggedInUserService.GetLoggedInUserIdAsync(), Is.EqualTo(1));
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.EqualTo("apikey-1"));
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.EqualTo("Brian"));
        }
    }
}
