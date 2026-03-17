using SQLite;

namespace HouseholdTracker.Core.Models;

/// <summary>
/// The class representing a household item
/// </summary>
public class Item
{
    /// <summary>
    /// The database line Id of the item
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// The Id of the user with the item
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The name/description of the item
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Which icon to use to represent it visually
    /// </summary>
    public int Icon { get; set; } = 0;
    /// <summary>
    /// Is the expiry date shown for this item
    /// </summary>
    public bool ExpiryVisible { get; set; } = true;
    /// <summary>
    /// The capacity of the item
    /// </summary>
    public int MaxSize { get; set; }
    /// <summary>
    /// The type of capacity of the item
    /// </summary>
    public ItemSizes ItemSize { get; set; } = ItemSizes.Grams;
    /// <summary>
    /// Displays the current level of an item. Avoids the use of decimals by declaring as int
    /// </summary>
    public int CurrentLevelAsPercentage { get; set; }
    /// <summary>
    /// The percentage at which the user should be notified of the item running lwo
    /// </summary>
    public int LowPercentageWarning { get; set; }
    /// <summary>
    /// The expiry date of the currently opened item
    /// </summary>
    public DateTime CurrentExpiry { get; set; } = DateTime.MinValue;
    /// <summary>
    /// The last time the item was updated, for use in keeping the data synchronised online
    /// </summary>
    public DateTime LastUpdateUTC { get; set; } = DateTime.MinValue;
    /// <summary>
    /// Whether or not the item is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;
    /// <summary>
    /// The position the item shold appear within its group
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// An empty constructor for the Item
    /// </summary>
    public Item() { }

#pragma warning disable S107
    /// <summary>
    /// Constructor for the item
    /// </summary>
    /// <param name="id">The lineId in the database for the item</param>
    /// <param name="userId">The UserId for the item</param>
    /// <param name="name">The name/description of the item</param>
    /// <param name="icon">The icon to be displayed visually</param>
    /// <param name="expiryVisible">Is the expiry date visible for the item</param>
    /// <param name="maxSize">The capacity of the current item</param>
    /// <param name="itemSize">The type of size of the item</param>
    /// <param name="currentLevelAsPercentage">The percentage level in stock of the item</param>
    /// <param name="lowPercentageWarning">The level at which the item should appear as needing re-purchasing</param>
    /// <param name="currentExpiry">The expiry date of the current item</param>
    /// <param name="lastUpdateUTC">The last time the item was updated</param>
    /// <param name="enabled">Whether or not the item is active</param>
    /// <param name="sortOrder">The position the item should be displayed within its group</param>
    public Item(int id, int userId, string name, int icon, bool expiryVisible, int maxSize, ItemSizes itemSize, int currentLevelAsPercentage, int lowPercentageWarning, DateTime currentExpiry, DateTime lastUpdateUTC, bool enabled, int sortOrder)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Icon = icon;
        ExpiryVisible = expiryVisible;
        MaxSize = maxSize;
        ItemSize = itemSize;
        CurrentLevelAsPercentage = currentLevelAsPercentage;
        LowPercentageWarning = lowPercentageWarning;
        CurrentExpiry = currentExpiry;
        LastUpdateUTC = lastUpdateUTC;
        Enabled = enabled;
        SortOrder = sortOrder;
    }
#pragma warning restore
}
