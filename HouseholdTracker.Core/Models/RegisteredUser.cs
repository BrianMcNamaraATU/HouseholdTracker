using SQLite;

namespace HouseholdTracker.Core.Models;

internal class RegisteredUser
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string APIKey { get; set; } = string.Empty;

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
