using SQLite;

namespace HouseholdTracker.Core.Models;

internal class RegisteredUser
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Username { get; internal set; } = string.Empty;
    public string Password { get; internal set; } = string.Empty;
    public string FirstName { get; internal set; } = string.Empty;
    public string LastName { get; internal set; } = string.Empty;
    public string Email { get; internal set; } = string.Empty;
    public string APIKey { get; internal set; } = string.Empty;

    public RegisteredUser() { }

    public RegisteredUser(int id, string username, string password, string firstName, string lastName, string email, string aPIKey)
    {
        Id = id;
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        APIKey = aPIKey;
    }
}