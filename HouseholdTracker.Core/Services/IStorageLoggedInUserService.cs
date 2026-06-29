namespace HouseholdTracker.Core.Services;

/// <summary>
/// Interface to be used as its not possible to run tests on SecureStorage
/// in a windows test environment.
/// The Interface ensures the actual and test implementations both have the same functionality
/// </summary>
public interface IStorageLoggedInUserService
{
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
