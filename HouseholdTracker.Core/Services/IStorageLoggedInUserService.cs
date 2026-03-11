namespace HouseholdTracker.Core.Services;

/// <summary>
/// Interface to be used as its not possible to run tests on Preferences or Secure Storage
/// in a windows test environment.
/// The Interface ensures the actual and test implementations both have the same functionality
/// </summary>
public interface IStorageLoggedInUserService
{
    /// <summary>
    /// Get the value of an item from the Preferences
    /// </summary>
    /// <param name="key">The key of the item</param>
    /// <param name="defaultValue">The default value if the key doesn't exist</param>
    /// <returns>The value of the item matching the key</returns>
    int GetPreference(string key, int defaultValue);

    /// <summary>
    /// Set the value of an item in the Preferences
    /// </summary>
    /// <param name="key">The key of the item to set</param>
    /// <param name="value">The value of the item to set</param>
    void SetPreference(string key, int value);

    /// <summary>
    /// Get the value of an item from the SecureStorage
    /// </summary>
    /// <param name="key">The key of the item</param>
    /// <returns>The value of the item matching the key</returns>
    Task<string?> GetSecureAsync(string key);

    /// <summary>
    /// Set the value of an item in the SecureStorage
    /// </summary>
    /// <param name="key">The key of the item to set</param>
    /// <param name="value">The value of the item to set</param>
    Task SetSecureAsync(string key, string value);
}
