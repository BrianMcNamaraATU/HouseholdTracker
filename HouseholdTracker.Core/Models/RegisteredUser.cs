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
    /// Their First Name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    /// <summary>
    /// Their Last Name
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    /// <summary>
    /// Their Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Their API Key, used for authenticating requests to the central database
    /// </summary>
    public string APIKey { get; set; } = string.Empty;
    /// <summary>
    /// Whether or not the user is required to reset their password on next login.
    /// Set to true when a password reset is requested.
    /// </summary>
    public bool ForcePasswordReset { get; set; } = false;
    /// <summary>
    /// Whether or not the user has verified their email address.
    /// Set to true once the user completes email verification.
    /// </summary>
    public bool EmailVerified { get; set; } = false;

    /// <summary>
    /// An empty constructor for the RegisteredUser class
    /// </summary>
    public RegisteredUser() { }

    /// <summary>
    /// A constructor for the RegisteredUser class
    /// </summary>
    /// <param name="id">Their id</param>
    /// <param name="firstName">Their first name</param>
    /// <param name="lastName">Their surname</param>
    /// <param name="email">Their email address</param>
    /// <param name="apiKey">Their API Key</param>
    /// <param name="forcePasswordReset">Whether or not the user must reset their password on next login</param>
    /// <param name="emailVerified">Whether or not the user has verified their email address</param>
    public RegisteredUser(int id, string firstName, string lastName, string email, string apiKey, bool forcePasswordReset = false, bool emailVerified = false)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        APIKey = apiKey;
        ForcePasswordReset = forcePasswordReset;
        EmailVerified = emailVerified;
    }
}
