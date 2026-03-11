using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the SharedAccountInvite class
/// </summary>
[TestFixture]
internal sealed class SharedAccountInviteTests
{
    /// <summary>
    /// Ensure the expected default values are set in an empty constructor
    /// </summary>
    [Test]
    public void DefaultConstructorTest()
    {
        var sharedAccount = new SharedAccountInvite();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(sharedAccount.Id, Is.Zero);
            Assert.That(sharedAccount.InvitedById, Is.Zero);
            Assert.That(sharedAccount.InviteSentToEmail, Is.EqualTo(string.Empty));
            Assert.That(sharedAccount.InviteCode, Is.EqualTo(string.Empty));
            Assert.That(sharedAccount.InvitedAt, Is.EqualTo(DateTime.MinValue));
            Assert.That(sharedAccount.AcceptedId, Is.Null);
            Assert.That(sharedAccount.AcceptedAt, Is.Null);
            Assert.That(sharedAccount.Accepted, Is.False);
            Assert.That(sharedAccount.SharedWith(0), Is.False);
            Assert.That(sharedAccount.SharedWith(1), Is.False);
        }
    }

    /// <summary>
    /// Ensure the correct values are set in a non-empty constructor
    /// </summary>
    [Test]
    public void DefaultSetterTest()
    {
        var tempTime = DateTime.Now;
        var sharedAccount = new SharedAccountInvite(1, 1, "brian@email.com", "8DIGITCODE", tempTime, 2, tempTime.AddHours(1));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(sharedAccount.Id, Is.EqualTo(1));
            Assert.That(sharedAccount.InvitedById, Is.EqualTo(1));
            Assert.That(sharedAccount.InviteSentToEmail, Is.EqualTo("brian@email.com"));
            Assert.That(sharedAccount.InviteCode, Is.EqualTo("8DIGITCODE"));
            Assert.That(sharedAccount.InvitedAt, Is.EqualTo(tempTime));
            Assert.That(sharedAccount.AcceptedId, Is.EqualTo(2));
            Assert.That(sharedAccount.AcceptedAt, Is.EqualTo(tempTime.AddHours(1)));
            Assert.That(sharedAccount.Accepted, Is.True);
            Assert.That(sharedAccount.SharedWith(0), Is.False);
            Assert.That(sharedAccount.SharedWith(1), Is.True);
            Assert.That(sharedAccount.SharedWith(2), Is.True);
        }
    }
}
