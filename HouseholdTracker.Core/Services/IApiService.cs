using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Core.Services;

/// <summary>
/// Interface for the API service used to communicate with the central database.
/// Allows for a test implementation to be used in place of the real implementation
/// </summary>
public interface IApiService
{
    /// <summary>
    /// Register a new user with the central database
    /// </summary>
    /// <param name="firstName">The user's first name</param>
    /// <param name="lastName">The user's last name</param>
    /// <param name="email">The user's email address</param>
    /// <param name="password">The user's chosen password</param>
    /// <returns>The registered user if successful, or null if registration failed</returns>
    Task<ApiResult<RegisteredUser>> RegisterAsync(string firstName, string lastName, string email, string password);

    /// <summary>
    /// Login an existing user with the central database
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <param name="password">The user's password</param>
    /// <returns>The logged in user if successful, or null if login failed</returns>
    Task<ApiResult<RegisteredUser>> LoginAsync(string email, string password);

    /// <summary>
    /// Request a password reset for a user
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <returns>A result indicating success or failure</returns>
    Task<ApiResult<bool>> ForgotPasswordAsync(string email);

    /// <summary>
    /// Change the password for a logged in user
    /// </summary>
    /// <param name="userId">The user's Id</param>
    /// <param name="apiKey">The user's API key</param>
    /// <param name="email">The user's email address</param>
    /// <param name="currentPassword">The user's current password</param>
    /// <param name="newPassword">The user's new password</param>
    /// <returns>A result indicating success or failure</returns>
    Task<ApiResult<bool>> ChangePasswordAsync(int userId, string apiKey, string email, string currentPassword, string newPassword);
}
