using SQLite;

namespace HouseholdTracker.Core.Models;

public class EmailValidation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string PreviousEmail { get; set; } = string.Empty;
    public string NewEmail { get; set; } = string.Empty;
    public string EmailCode { get; set; } = string.Empty;
    public DateTime SentUTC { get; set; } = DateTime.MinValue;
    public DateTime? ValidatedUTC { get; set; } = null;

    public EmailValidation() { }

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
