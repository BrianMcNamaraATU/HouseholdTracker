using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// A test implementation of IApiService that returns configurable
/// results without making real HTTP calls
/// </summary>
public class TestApiService : IApiService
{
    /// <summary>The result to return from RegisterAsync</summary>
    public ApiResult<RegisteredUser>? RegisterResult { get; set; }

    /// <summary>The result to return from LoginAsync</summary>
    public ApiResult<RegisteredUser>? LoginResult { get; set; }

    /// <summary>The result to return from ForgotPasswordAsync</summary>
    public ApiResult<bool>? ForgotPasswordResult { get; set; }

    /// <summary>The result to return from ChangePasswordAsync</summary>
    public ApiResult<bool>? ChangePasswordResult { get; set; }

    /// <summary>
    /// Returns the configured RegisterResult
    /// </summary>
    public Task<ApiResult<RegisteredUser>> RegisterAsync(string firstName, string lastName, string email, string password) =>
        Task.FromResult(RegisterResult ?? ApiResult<RegisteredUser>.Fail("Not configured."));

    /// <summary>
    /// Returns the configured LoginResult
    /// </summary>
    public Task<ApiResult<RegisteredUser>> LoginAsync(string email, string password) =>
        Task.FromResult(LoginResult ?? ApiResult<RegisteredUser>.Fail("Not configured."));

    /// <summary>
    /// Returns the configured ForgotPasswordResult
    /// </summary>
    public Task<ApiResult<bool>> ForgotPasswordAsync(string email) =>
        Task.FromResult(ForgotPasswordResult ?? ApiResult<bool>.Fail("Not configured."));

    /// <summary>
    /// Returns the configured ChangePasswordResult
    /// </summary>
    public Task<ApiResult<bool>> ChangePasswordAsync(int userId, string apiKey, string email, string currentPassword, string newPassword) =>
        Task.FromResult(ChangePasswordResult ?? ApiResult<bool>.Fail("Not configured."));
}
