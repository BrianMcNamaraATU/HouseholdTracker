using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// Class to represent the LoggedInUserStorage for testing purposes
/// Its not possible to use the actual Preferences and SecureStorage as
/// windows cannot replicate these.
/// </summary>
public class TestStorageLoggedInUserService : IStorageLoggedInUserService
{
    private readonly Dictionary<string, object> _store = [];

    /// <summary>
    /// Get the value of an item from the Preferences
    /// </summary>
    /// <param name="key">The key of the item</param>
    /// <param name="defaultValue">The default value if the key doesn't exist</param>
    /// <returns>The value of the item matching the key</returns>
    public int GetPreference(string key, int defaultValue) =>
        _store.TryGetValue(key, out var val) ? (int)val : defaultValue;

    /// <summary>
    /// Set the value of an item in the Preferences
    /// </summary>
    /// <param name="key">The key of the item to set</param>
    /// <param name="value">The value of the item to set</param>
    public void SetPreference(string key, int value) => _store[key] = value;

    /// <summary>
    /// Get the value of an item from the SecureStorage
    /// </summary>
    /// <param name="key">The key of the item</param>
    /// <returns>The value of the item matching the key</returns>
    public Task<string?> GetSecureAsync(string key) =>
        Task.FromResult(_store.TryGetValue(key, out var val) ? (string?)val : null);

    /// <summary>
    /// Set the value of an item in the SecureStorage
    /// </summary>
    /// <param name="key">The key of the item to set</param>
    /// <param name="value">The value of the item to set</param>
    public Task SetSecureAsync(string key, string value)
    {
        _store[key] = value;
        return Task.CompletedTask;
    }
}
