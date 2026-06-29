namespace HouseholdTracker.Core;

/// <summary>
/// A static class containing constant values used throughout the application
/// </summary>
public static class Constants
{
    /// <summary>
    /// The API key used by the app to authenticate requests to the central database.
    /// This is the system-level key, separate from individual user API keys.
    /// </summary>
    public const string AppApiKey = "f22144d63501e2e6c8306fbe459ae234177bab511d092177b44e6e8fab1207ff";

    /// <summary>
    /// The base URL for all central database API requests
    /// </summary>
    public const string ApiBaseUrl = "https://www.cdcsoftware.ie/HTracker/api";

    /// <summary>
    /// The key used to store and retrieve the logged in user's Id from SecureStorage
    /// </summary>
    public const string StorageKeyUserId = "UserId";

    /// <summary>
    /// The key used to store and retrieve the logged in user's API key from SecureStorage
    /// </summary>
    public const string StorageKeyApiKey = "apiKey";

    /// <summary>
    /// The key used to store and retrieve the logged in user's first name from SecureStorage
    /// </summary>
    public const string StorageKeyFirstName = "firstName";

    /// <summary>
    /// The key used to store and retrieve the logged in user's email address from SecureStorage
    /// </summary>
    public const string StorageKeyEmail = "email";
}
