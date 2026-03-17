using SQLite;

namespace HouseholdTracker.Core.Models;

/// <summary>
/// The class representing a RegisteredUser
/// </summary>
public class RegisteredUser
{
    /// <summary>
    /// The Id of the RegisteredUser
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// Their Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// Their Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// Their FirstName
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// Their LastName
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// Their Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Their APIKey
    /// </summary>
    public string APIKey { get; set; } = string.Empty;

    /// <summary>
    /// An empty constructor for the RegisteredUser class
    /// </summary>
    public RegisteredUser() { }

    /// <summary>
    /// A constructor for the RegisteredUser class
    /// </summary>
    /// <param name="id">Their id</param>
    /// <param name="username">Their username</param>
    /// <param name="password">Their password</param>
    /// <param name="firstName">Their firstname</param>
    /// <param name="lastName">Their surname</param>
    /// <param name="email">Their email address</param>
    /// <param name="aPIKey">Their APIKey</param>
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
