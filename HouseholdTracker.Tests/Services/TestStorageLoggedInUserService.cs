using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// Class to represent the LoggedInUserStorage for testing purposes.
/// It is not possible to use the actual SecureStorage in a Windows test
/// environment. The Interface ensures the actual and test implementations
/// both have the same functionality
/// </summary>
public class TestStorageLoggedInUserService : IStorageLoggedInUserService
{
    private readonly Dictionary<string, string> _store = [];

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
