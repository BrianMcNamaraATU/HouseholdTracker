using SQLite;

namespace HouseholdTracker.Core.Models;

internal class UserPreferences
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int UserId { get; set; }
    public Preferences Preference { get; set; }
    public bool Value { get; set; } = false;

    public UserPreferences() { }

    public UserPreferences(int id, int userId, Preferences preference, bool value)
    {
        Id = id;
        UserId = userId;
        Preference = preference;
        Value = value;
    }
}

internal enum Preferences
{
    None = 0,
    Marketing = 1,
    SharedAccountAcceptance = 2
}
