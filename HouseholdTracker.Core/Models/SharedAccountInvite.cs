using SQLite;

namespace HouseholdTracker.Core.Models;

internal class SharedAccountInvite
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int InvitedById { get; set; }
    public string InviteSentToEmail { get; set; } = string.Empty;
    public string InviteCode { get; set; } = string.Empty;
    public DateTime InvitedAt { get; set; } = DateTime.MinValue;
    public int? AcceptedId { get; set; }
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// A boolean representation of whether the Invitation has been accepted or not
    /// </summary>
    public bool Accepted => AcceptedAt != null;
    /// <summary>
    /// A boolean representation of whether the userId in the parameter is part of a shared account
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool SharedWith(int userId) => Accepted && (InvitedById == userId || AcceptedId == userId);

    public SharedAccountInvite() { }

    public SharedAccountInvite(int id, int invitedById, string inviteSentToEmail, string inviteCode, DateTime invitedAt, int? acceptedId, DateTime? acceptedAt)
    {
        Id = id;
        InvitedById = invitedById;
        InviteSentToEmail = inviteSentToEmail;
        InviteCode = inviteCode;
        InvitedAt = invitedAt;
        AcceptedId = acceptedId;
        AcceptedAt = acceptedAt;
    }
}
