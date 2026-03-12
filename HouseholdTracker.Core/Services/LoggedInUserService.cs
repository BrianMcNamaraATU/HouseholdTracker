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

    internal static int? LoggedInUserId
    {
        get
        {
            var id = s_storage.GetPreference("UserId", 0);
            return id != 0 ? id : null;
        }
    }

    internal static async Task<string?> GetLoggedInUserFirstNameAsync() =>
        await s_storage.GetSecureAsync("firstName");

    internal static async Task<string?> GetLoggedInUserAPIKeyAsync() =>
        await s_storage.GetSecureAsync("apiKey");

    internal static async Task LoginUserAsync(RegisteredUser activeUser)
    {
        s_storage.SetPreference("UserId", activeUser.Id);
        await s_storage.SetSecureAsync("apiKey", activeUser.APIKey);
        await s_storage.SetSecureAsync("firstName", activeUser.FirstName);
    }

    internal static async Task LogoutUserAsync()
    {
        s_storage.SetPreference("UserId", 0);
        await s_storage.SetSecureAsync("apiKey", "");
        await s_storage.SetSecureAsync("firstName", "");
    }
}
