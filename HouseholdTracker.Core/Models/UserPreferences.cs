using SQLite;

namespace HouseholdTracker.Core.Models;

/// <summary>
/// The class representing UserPreferences
/// </summary>
public class UserPreferences
{
    /// <summary>
    /// The Id within the central database of the UserPreference
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// The UserId of the UserPrefernce
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The Preference type
    /// </summary>
    public Preferences Preference { get; set; }
    /// <summary>
    /// The value of the UserPreference
    /// </summary>
    public bool Value { get; set; } = false;

    /// <summary>
    /// An empty constructor for the UserPreference class
    /// </summary>
    public UserPreferences() { }

    /// <summary>
    /// A constructor for the UserPreference class
    /// </summary>
    /// <param name="id">The id within the central database</param>
    /// <param name="userId">The UserId of the preference</param>
    /// <param name="preference">The preference type</param>
    /// <param name="value">The value of the preference</param>
    public UserPreferences(int id, int userId, Preferences preference, bool value)
    {
        Id = id;
        UserId = userId;
        Preference = preference;
        Value = value;
    }
}

/// <summary>
/// The types of Preferences available
/// </summary>
public enum Preferences
{
    /// <summary>
    /// The default type
    /// </summary>
    None = 0,
    /// <summary>
    /// Marketing emails are allowed/disallowed
    /// </summary>
    Marketing = 1,
    /// <summary>
    /// Emails will be received when someone accepts a shared account invite by the user
    /// </summary>
    SharedAccountAcceptance = 2
}
