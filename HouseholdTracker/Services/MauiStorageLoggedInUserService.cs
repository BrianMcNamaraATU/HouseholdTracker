using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Services;

/// <summary>
/// The actual storage solution to use in the apps
/// Makes use of Preferences for low priority data security
/// and SecureStorage for high priority
/// </summary>
public class MauiStorageLoggedInUserService : IStorageLoggedInUserService
{
    /// <summary>
    /// Get the value of an item from the Preferences
    /// </summary>
    /// <param name="key">The key of the item</param>
    /// <param name="defaultValue">The default value if the key doesn't exist</param>
    /// <returns>The value of the item matching the key</returns>
    public int GetPreference(string key, int defaultValue) => Preferences.Get(key, defaultValue);

    /// <summary>
    /// Set the value of an item in the Preferences
    /// </summary>
    /// <param name="key">The key of the item to set</param>
    /// <param name="value">The value of the item to set</param>
    public void SetPreference(string key, int value) => Preferences.Set(key, value);

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
