using SQLite;

namespace HouseholdTracker.Core.Models;

internal class EmailValidation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; private set; }
    public int UserId { get; internal set; }
    public string PreviousEmail { get; internal set; } = string.Empty;
    public string NewEmail { get; internal set; } = string.Empty;
    public string EmailCode { get; internal set; } = string.Empty;
    public DateTime SentUTC { get; internal set; } = DateTime.MinValue;
    public DateTime? ValidatedUTC { get; internal set; } = null;

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
