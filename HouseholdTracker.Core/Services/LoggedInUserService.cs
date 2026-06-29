using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Core.Services;

/// <summary>
/// A static class for use in retrieving various attributes of the currently
/// logged in user
/// </summary>
internal static class LoggedInUserService
{
    private static IStorageLoggedInUserService s_storage = null!;

    /// <summary>
    /// The storage type to be used. Necessary as testing and production will
    /// use different types
    /// </summary>
    /// <param name="storage">The interface of the storage type to be used</param>
    internal static void Initialize(IStorageLoggedInUserService storage)
    {
        s_storage = storage;
    }

    /// <summary>
    /// Get the Id of the currently logged in user from SecureStorage,
    /// or null if no user is logged in
    /// </summary>
    /// <returns>The Id of the logged in user, or null if not set</returns>
    internal static async Task<int?> GetLoggedInUserIdAsync()
    {
        var value = await s_storage.GetSecureAsync(Constants.StorageKeyUserId);
        return int.TryParse(value, out var id) && id != 0 ? id : null;
    }

    /// <summary>
    /// Get the first name of the currently logged in user from SecureStorage
    /// </summary>
    /// <returns>The first name of the logged in user, or null if not set</returns>
    internal static async Task<string?> GetLoggedInUserFirstNameAsync() =>
        await s_storage.GetSecureAsync(Constants.StorageKeyFirstName);

    /// <summary>
    /// Get the email address of the currently logged in user from SecureStorage
    /// </summary>
    /// <returns>The email address of the logged in user, or null if not set</returns>
    internal static async Task<string?> GetLoggedInUserEmailAsync() =>
        await s_storage.GetSecureAsync(Constants.StorageKeyEmail);

    /// <summary>
    /// Get the API key of the currently logged in user from SecureStorage
    /// </summary>
    /// <returns>The API key of the logged in user, or null if not set</returns>
    internal static async Task<string?> GetLoggedInUserAPIKeyAsync() =>
        await s_storage.GetSecureAsync(Constants.StorageKeyApiKey);

    /// <summary>
    /// Store the logged in user's details in SecureStorage
    /// </summary>
    /// <param name="activeUser">The RegisteredUser who has logged in</param>
    internal static async Task LoginUserAsync(RegisteredUser activeUser)
    {
        await s_storage.SetSecureAsync(Constants.StorageKeyUserId, activeUser.Id.ToString());
        await s_storage.SetSecureAsync(Constants.StorageKeyApiKey, activeUser.APIKey);
        await s_storage.SetSecureAsync(Constants.StorageKeyFirstName, activeUser.FirstName);
        await s_storage.SetSecureAsync(Constants.StorageKeyEmail, activeUser.Email);
    }

    /// <summary>
    /// Clear the logged in user's details from SecureStorage
    /// </summary>
    internal static async Task LogoutUserAsync()
    {
        await s_storage.SetSecureAsync(Constants.StorageKeyUserId, string.Empty);
        await s_storage.SetSecureAsync(Constants.StorageKeyApiKey, string.Empty);
        await s_storage.SetSecureAsync(Constants.StorageKeyFirstName, string.Empty);
        await s_storage.SetSecureAsync(Constants.StorageKeyEmail, string.Empty);
    }
}
