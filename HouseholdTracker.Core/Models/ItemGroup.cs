using SQLite;

namespace HouseholdTracker.Core.Models;

/// <summary>
/// The class for the ItemGroup
/// </summary>
public class ItemGroup
{
    /// <summary>
    /// The id of the ItemGroup
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// The UserId of the ItemGroup
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The name/description of the ItemGroup
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The default icon to be used visually for all items within that group
    /// </summary>
    public int DefaultIcon { get; set; } = 0;
    /// <summary>
    /// The default item size type to be used for all items within that group
    /// </summary>
    public ItemSizes DefaultItemSize { get; set; } = ItemSizes.Grams;
    /// <summary>
    /// Whether the expiry date is visible as default for all items within that group
    /// </summary>
    public bool DefaultExpiryVisible { get; set; } = true;
    /// <summary>
    /// The default low percentage warning value for all items within that group
    /// </summary>
    public int DefaultLowPercentageWarning { get; set; } = 25;
    /// <summary>
    /// Whether or not the group is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;
    /// <summary>
    /// Whether or not the group has been synchronised with the central database
    /// </summary>
    public bool OnlineMatched { get; set; } = true;
    /// <summary>
    /// The positioning of this group visually on the users page
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// An empty constructor for the ItemGroup
    /// </summary>
    public ItemGroup() { }

#pragma warning disable S107
    /// <summary>
    /// Constructor for the ItemGroup
    /// </summary>
    /// <param name="id">The lineId in the database of the ItemGroup</param>
    /// <param name="userID">The Id of the User with the ItemGroup</param>
    /// <param name="name">The name/description of the ItemGroup</param>
    /// <param name="defaultIcon">The default icon for the ItemGroup</param>
    /// <param name="defaultItemSize">The default item size type for the ItemGroup</param>
    /// <param name="defaultExpiryVisible">The default value of whether the expiry is visible or not</param>
    /// <param name="defaultLowPercentageWarning">The default value for when Items within the Group should notify the user</param>
    /// <param name="enabled">Whether or not the ItemGroup is enabled or not</param>
    /// <param name="onlineMatched">Whether or not the ItemGroup has been synchronised with the central database</param>
    /// <param name="sortOrder">The position on the screen of the ItemGroup</param>
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

/// <summary>
/// The measurement type of the item
/// </summary>
public enum ItemSizes
{
    /// <summary>
    /// Grams
    /// </summary>
    Grams = 1,
    /// <summary>
    /// Mililitres
    /// </summary>
    ML = 2
};
