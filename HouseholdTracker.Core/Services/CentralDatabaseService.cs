using HouseholdTracker.Core.Models;
using SQLite;

namespace HouseholdTracker.Core.Services;

/// <summary>
/// The Central Database Service
/// This will provide connectivity to the central database where all users data is stored
/// </summary>
/// <remarks>
/// The constructor for the Central Database Service
/// </remarks>
/// <param name="dbPath">The path to the central database</param>
public class CentralDatabaseService(string dbPath) : IAsyncDisposable
{
    private readonly SQLiteAsyncConnection _db = new(dbPath);

    /// <summary>
    /// Ensures creation of all necessary tables
    /// </summary>
    public async Task InitAsync()
    {
        await _db.CreateTableAsync<RegisteredUser>().ConfigureAwait(true);
    }

    #region "RegisteredUser"
    /// <summary>
    /// Method to test if the credentials for the user are valid
    /// This is to prevent data from one user being shown to another by
    /// modifying the stored Id of the user
    /// </summary>
    /// <param name="id">The Id of the user</param>
    /// <param name="APIKey">The API Key stored for the specific user</param>
    /// <returns>True if the credentials are valid, false if they aren't</returns>
    internal async Task<bool> UserIsValid(int id, string APIKey)
    {
        var user = await _db.Table<RegisteredUser>()
            .Where(ru => ru.Id == id && ru.APIKey == APIKey)
            .FirstOrDefaultAsync().ConfigureAwait(false);

        return user != null;
    }

    /// <summary>
    /// Method to test if the login credentials for the user are valid
    /// </summary>
    /// <param name="Username">The Username entered</param>
    /// <param name="Password">The Password entered</param>
    /// <returns>The RegisteredUser if the credentials match, or null if they don't</returns>
    internal async Task<RegisteredUser> UserIsValid(string Username, string Password)
    {
        return await _db.Table<RegisteredUser>()
            .Where(ru => ru.Username == Username && ru.Password == Password)
            .FirstOrDefaultAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Method to get the count of Registered Users on the central database
    /// Primarily will be used during testing
    /// </summary>
    /// <returns>The number of Registered Users in the central database</returns>
    internal Task<int> GetRegisteredUserCountAsync() =>
        _db.Table<RegisteredUser>().CountAsync();

    /// <summary>
    /// Method to add a new Registered User to the central database
    /// </summary>
    /// <param name="user">The new Registered User to add</param>
    /// <returns></returns>
    internal Task<int> AddRegisteredUserAsync(RegisteredUser user) =>
        _db.InsertAsync(user);

    /// <summary>
    /// Method to update a Registered User on the central database
    /// </summary>
    /// <param name="user">The Registered User to update</param>
    /// <returns></returns>
    internal Task<int> UpdateRegisteredUserAsync(RegisteredUser user) =>
        _db.UpdateAsync(user);
    #endregion


    /// <summary>
    /// Dispose of the CentralDatabaseService
    /// Mostly needed for testing purposes for clearing the database between tests
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _db.CloseAsync();
    }
}
