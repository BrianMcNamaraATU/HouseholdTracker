using SQLite;

namespace HouseholdTracker.Core.Models;

public class Item
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Icon { get; set; } = 0;
    public bool ExpiryVisible { get; set; } = true;
    public int MaxSize { get; set; }
    public ItemSizes ItemSize { get; set; } = ItemSizes.Grams;
    /// <summary>
    /// Displays the current level of an item. Avoids the use of decimals by declaring as int
    /// </summary>
    public int CurrentLevelAsPercentage { get; set; }
    public int LowPercentageWarning { get; set; }
    public DateTime CurrentExpiry { get; set; } = DateTime.MinValue;
    public DateTime LastUpdateUTC { get; set; } = DateTime.MinValue;
    public bool Enabled { get; set; } = true;
    public int SortOrder { get; set; } = 0;

    public Item() { }

#pragma warning disable S107
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
