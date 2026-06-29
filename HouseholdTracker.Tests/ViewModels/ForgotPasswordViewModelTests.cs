using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;
using HouseholdTracker.Core.ViewModels;
using HouseholdTracker.Tests.Services;

namespace HouseholdTracker.Tests.ViewModels;

/// <summary>
/// Tests for the ForgotPasswordViewModel
/// </summary>
[TestFixture]
internal sealed class ForgotPasswordViewModelTests
{
    private TestApiService _apiService;
    private ForgotPasswordViewModel _viewModel;

    /// <summary>
    /// Create fresh instances before each test
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        _apiService = new TestApiService();
        _viewModel = new ForgotPasswordViewModel(_apiService);
    }

    /// <summary>
    /// Ensure an empty email shows an error
    /// </summary>
    [Test]
    public async Task EmptyEmail_ShowsEmailError()
    {
        _viewModel.Email = string.Empty;
        await _viewModel.SubmitCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an invalid email format shows an error
    /// </summary>
    [Test]
    public async Task InvalidEmail_ShowsEmailError()
    {
        _viewModel.Email = "notanemail";
        await _viewModel.SubmitCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure an email over 50 characters shows an error
    /// </summary>
    [Test]
    public async Task EmailTooLong_ShowsEmailError()
    {
        _viewModel.Email = new string('a', 45) + "@test.com";
        await _viewModel.SubmitCommand.ExecuteAsync(null);
        Assert.That(_viewModel.EmailError, Is.Not.Empty);
    }

    /// <summary>
    /// Ensure a valid submission sets SubmitSuccessful regardless of API result
    /// </summary>
    [Test]
    public async Task ValidEmail_SetsSubmitSuccessful()
    {
        _apiService.ForgotPasswordResult = ApiResult<bool>.Ok(true);
        _viewModel.Email = "brian@example.com";
        await _viewModel.SubmitCommand.ExecuteAsync(null);
        Assert.That(_viewModel.SubmitSuccessful, Is.True);
    }

    /// <summary>
    /// Ensure SubmitSuccessful is set even when the API returns a failure,
    /// to avoid exposing which emails are registered
    /// </summary>
    [Test]
    public async Task ValidEmail_SetsSubmitSuccessfulEvenOnApiFailure()
    {
        _apiService.ForgotPasswordResult = ApiResult<bool>.Fail("Not found.");
        _viewModel.Email = "unknown@example.com";
        await _viewModel.SubmitCommand.ExecuteAsync(null);
        Assert.That(_viewModel.SubmitSuccessful, Is.True);
    }
}
