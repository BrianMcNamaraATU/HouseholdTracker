using SQLite;

namespace HouseholdTracker.Core.Models;

/// <summary>
/// The class which will track whether or not the user has verified their email address or not
/// </summary>
public class EmailValidation
{
    /// <summary>
    /// The incremental Id in the database
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// The UserId of the user
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// The users previous email address for when they change their address
    /// </summary>
    public string PreviousEmail { get; set; } = string.Empty;
    /// <summary>
    /// The users new email address
    /// </summary>
    public string NewEmail { get; set; } = string.Empty;
    /// <summary>
    /// The code sent to the user to verify their email address
    /// </summary>
    public string EmailCode { get; set; } = string.Empty;
    /// <summary>
    /// The timestamp of when the email address was changed
    /// </summary>
    public DateTime SentUTC { get; set; } = DateTime.MinValue;
    /// <summary>
    /// The timestamp of when the email address was verified
    /// </summary>
    public DateTime? ValidatedUTC { get; set; } = null;
  
    /// <summary>
    /// Default constructor, needed for SQLite integration
    /// </summary>
    public EmailValidation() { }

    /// <summary>
    /// Constructor with all parameters
    /// </summary>
    /// <param name="id">The lineId in the database</param>
    /// <param name="userId">The UserId of the user</param>
    /// <param name="previousEmail">The users previous email address (if applicable)</param>
    /// <param name="newEmail">The users new email address</param>
    /// <param name="emailCode">The code sent to the user to verify their address</param>
    /// <param name="sentUTC">The timestamp of when the email address was changed</param>
    /// <param name="validatedUTC">The timestamp of when the email address was verified</param>
    public EmailValidation(int id, int userId, string previousEmail, string newEmail, string emailCode, DateTime sentUTC, DateTime? validatedUTC)
    {
        Id = id;
        UserId = userId;
        PreviousEmail = previousEmail;
        NewEmail = newEmail;
        EmailCode = emailCode;
        SentUTC = sentUTC;
        ValidatedUTC = validatedUTC;
    }
}
