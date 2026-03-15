using HouseholdTracker.Core.Models;
using SQLite;

namespace HouseholdTracker.Core.Services;

/// <summary>
/// The Local Database Service
/// This will provide storage when the user has no access to the internet (Cental Database)
/// </summary>
/// <remarks>
/// The constructor for the Local Database Service
/// </remarks>
/// <param name="dbPath">The path to the local database</param>
public class LocalDatabaseService(string dbPath) : IAsyncDisposable
{
    private readonly SQLiteAsyncConnection _db = new(dbPath);

    /// <summary>
    /// Ensures creation of all necessary tables
    /// </summary>
    public async Task InitAsync()
    {
        await _db.CreateTableAsync<Item>().ConfigureAwait(true);
        await _db.CreateTableAsync<ItemGroup>().ConfigureAwait(true);
    }

    #region "ItemGroup"
    /// <summary>
    /// Method to get all of the Item Groups on the local database
    /// The UserId does not matter, as this will be controlled elsewhere to
    /// ensure only this users Item Groups are pulled down from the server
    /// </summary>
    /// <returns>The Item Groups in the local database</returns>
    internal Task<List<ItemGroup>> GetItemGroupsAsync() =>
        _db.Table<ItemGroup>().OrderBy(ig => ig.SortOrder).ToListAsync();

    /// <summary>
    /// Method to get all active Item Groups on the local database
    /// The UserId does not matter, as this will be controlled elsewhere to
    /// ensure only this users Item Groups are pulled down from the server
    /// </summary>
    /// <returns>The Item Groups in the local database</returns>
    internal Task<List<ItemGroup>> GetItemGroupsActiveAsync() =>
        _db.Table<ItemGroup>().Where(ig => ig.Enabled).OrderBy(ig => ig.SortOrder).ToListAsync();

    /// <summary>
    /// Method to get the count of Item Groups on the local database
    /// </summary>
    /// <returns>The number of Item Groups in the local database</returns>
    internal Task<int> GetItemGroupCountAsync() =>
        _db.Table<ItemGroup>().CountAsync();

    /// <summary>
    /// Method to add a new Item Group to the local database
    /// </summary>
    /// <param name="group">The Item Group to add</param>
    /// <returns></returns>
    internal Task<int> AddItemGroupAsync(ItemGroup group) =>
        _db.InsertAsync(group);

    /// <summary>
    /// Method to update an Item Group on the local database
    /// </summary>
    /// <param name="group">The Item Group to update</param>
    /// <returns></returns>
    internal Task<int> UpdateItemGroupAsync(ItemGroup group) =>
        _db.UpdateAsync(group);

    /// <summary>
    /// Method to delete a specific Item Group from the local database
    /// </summary>
    /// <param name="group">The Item Group to delete</param>
    /// <returns></returns>
    internal Task<int> DeleteItemGroupAsync(ItemGroup group) =>
        _db.DeleteAsync(group);

    /// <summary>
    /// Method to delete all Item Groups from the local database
    /// </summary>
    /// <returns></returns>
    internal Task<int> DeleteItemGroupsAllAsync() =>
        _db.DeleteAllAsync<ItemGroup>();
    #endregion

    #region "Item"
    /// <summary>
    /// Method to get all of the Items on the local database
    /// The UserId does not matter, as this will be controlled elsewhere to
    /// ensure only this users Items are pulled down from the server
    /// </summary>
    /// <returns>The Items in the local database</returns>
    internal Task<List<Item>> GetItemAsync() =>
        _db.Table<Item>().ToListAsync();

    /// <summary>
    /// Method to get all active Items on the local database
    /// The UserId does not matter, as this will be controlled elsewhere to
    /// ensure only this users Items are pulled down from the server
    /// </summary>
    /// <returns>The Items in the local database</returns>
    internal Task<List<Item>> GetItemActiveAsync() =>
        _db.Table<Item>().Where(itm => itm.Enabled).ToListAsync();

    /// <summary>
    /// Method to get the count of Items on the local database
    /// </summary>
    /// <returns>The number of Items in the local database</returns>
    internal Task<int> GetItemCountAsync() =>
        _db.Table<Item>().CountAsync();

    /// <summary>
    /// Method to add a new Item to the local database
    /// </summary>
    /// <param name="itm">The Item to add</param>
    /// <returns></returns>
    internal Task<int> AddItemAsync(Item itm) =>
        _db.InsertAsync(itm);

    /// <summary>
    /// Method to update an Item on the local database
    /// </summary>
    /// <param name="itm">The Item to update</param>
    /// <returns></returns>
    internal Task<int> UpdateItemAsync(Item itm) =>
        _db.UpdateAsync(itm);

    /// <summary>
    /// Method to delete a specific Item from the local database
    /// </summary>
    /// <param name="itm">The Item to delete</param>
    /// <returns></returns>
    internal Task<int> DeleteItemAsync(Item itm) =>
        _db.DeleteAsync(itm);

    /// <summary>
    /// Method to delete all Items from the local database
    /// </summary>
    /// <returns></returns>
    internal Task<int> DeleteItemAllAsync() =>
        _db.DeleteAllAsync<Item>();
    #endregion

    /// <summary>
    /// Dispose of the LocalDatabaseService
    /// Mostly needed for testing purposes for clearing the database between tests
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await _db.CloseAsync();
    }
}
