using SQLite;

namespace HouseholdTracker.Core.Models;

/// <summary>
/// The class representing shared profiles between users
/// </summary>
public class SharedAccountInvite
{
    /// <summary>
    /// The Id of the link
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    /// <summary>
    /// The Id of the RegisteredUser who invited the other RegisteredUser
    /// </summary>
    public int InvitedById { get; set; }
    /// <summary>
    /// The email address that the invite was sent to
    /// </summary>
    public string InviteSentToEmail { get; set; } = string.Empty;
    /// <summary>
    /// The invite code sent to the email address
    /// </summary>
    public string InviteCode { get; set; } = string.Empty;
    /// <summary>
    /// The timestamp that the invite was made
    /// </summary>
    public DateTime InvitedAt { get; set; } = DateTime.MinValue;
    /// <summary>
    /// The id of the RegisteredUser who accepted the profile share
    /// </summary>
    public int? AcceptedId { get; set; }
    /// <summary>
    /// The timestamp that the invitation was accepted
    /// </summary>
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

    /// <summary>
    /// An empty constructor for the SharedAccountInvite class
    /// </summary>
    public SharedAccountInvite() { }

    /// <summary>
    /// A constructor for the SharedAccountInvite class
    /// </summary>
    /// <param name="id">The id of the invite</param>
    /// <param name="invitedById">The id of the user who initiated the invite</param>
    /// <param name="inviteSentToEmail">The email address the invite was sent to</param>
    /// <param name="inviteCode">The code that was sent to the email address</param>
    /// <param name="invitedAt">The timestamp of when the invite was sent</param>
    /// <param name="acceptedId">The userId of the user who accepted the invite</param>
    /// <param name="acceptedAt">The timestamp of when the invite was accepted</param>
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
