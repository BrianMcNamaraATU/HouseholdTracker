using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Services;

/// <summary>
/// The actual storage solution to use in the app.
/// Makes use of SecureStorage for all data, ensuring encryption
/// on both Android (Keystore) and iOS (Keychain)
/// </summary>
public class MauiStorageLoggedInUserService : IStorageLoggedInUserService
{
    /// <summary>
    /// Get the value of an item from the SecureStorage
    /// </summary>
    /// <param name="key">The key of the item</param>
    /// <returns>The value of the item matching the key</returns>
    public Task<string?> GetSecureAsync(string key) => SecureStorage.GetAsync(key);

    /// <summary>
    /// Set the value of an item in the SecureStorage
    /// </summary>
    /// <param name="key">The key of the item to set</param>
    /// <param name="value">The value of the item to set</param>
    public Task SetSecureAsync(string key, string value) => SecureStorage.SetAsync(key, value);
}
