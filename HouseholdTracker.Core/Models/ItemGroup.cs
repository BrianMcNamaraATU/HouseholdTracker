using SQLite;

namespace HouseholdTracker.Core.Models;

public class ItemGroup
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DefaultIcon { get; set; } = 0;
    public ItemSizes DefaultItemSize { get; set; } = ItemSizes.Grams;
    public bool DefaultExpiryVisible { get; set; } = true;
    public int DefaultLowPercentageWarning { get; set; } = 25;
    public bool Enabled { get; set; } = true;
    public bool OnlineMatched { get; set; } = true;
    public int SortOrder { get; set; } = 0;

    public ItemGroup() { }

#pragma warning disable S107
    public ItemGroup(int id, int userID, string name, int defaultIcon, ItemSizes defaultItemSize, bool defaultExpiryVisible, int defaultLowPercentageWarning, bool enabled, bool onlineMatched, int sortOrder)
    {
        Id = id;
        UserId = userID;
        Name = name;
        DefaultIcon = defaultIcon;
        DefaultItemSize = defaultItemSize;
        DefaultExpiryVisible = defaultExpiryVisible;
        DefaultLowPercentageWarning = defaultLowPercentageWarning;
        Enabled = enabled;
        OnlineMatched = onlineMatched;
        SortOrder = sortOrder;
    }
#pragma warning restore
}

public enum ItemSizes
{
    Grams = 1,
    ML = 2
};
