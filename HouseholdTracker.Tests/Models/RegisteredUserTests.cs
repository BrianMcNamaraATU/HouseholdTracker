using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the RegisteredUser class
/// </summary>
[TestFixture]
internal sealed class RegisteredUserTests
{
    /// <summary>
    /// Ensure the expected default values are used in an empty constructor
    /// </summary>
    [Test]
    public void DefaultConstructorTest()
    {
        var registeredUser = new RegisteredUser();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(registeredUser.Id, Is.Zero);
            Assert.That(registeredUser.FirstName, Is.EqualTo(string.Empty));
            Assert.That(registeredUser.LastName, Is.EqualTo(string.Empty));
            Assert.That(registeredUser.Email, Is.EqualTo(string.Empty));
            Assert.That(registeredUser.APIKey, Is.EqualTo(string.Empty));
            Assert.That(registeredUser.ForcePasswordReset, Is.False);
            Assert.That(registeredUser.EmailVerified, Is.False);
        }
    }

    /// <summary>
    /// Ensure the values are set correctly in a non-empty constructor using default values
    /// for ForcePasswordReset and EmailVerified
    /// </summary>
    [Test]
    public void DefaultSetterTest()
    {
        var registeredUser = new RegisteredUser(1, "Brian", "McNamara", "brian@email.com", "123456abcdef");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(registeredUser.Id, Is.EqualTo(1));
            Assert.That(registeredUser.FirstName, Is.EqualTo("Brian"));
            Assert.That(registeredUser.LastName, Is.EqualTo("McNamara"));
            Assert.That(registeredUser.Email, Is.EqualTo("brian@email.com"));
            Assert.That(registeredUser.APIKey, Is.EqualTo("123456abcdef"));
            Assert.That(registeredUser.ForcePasswordReset, Is.False);
            Assert.That(registeredUser.EmailVerified, Is.False);
        }
    }

    /// <summary>
    /// Ensure the values are set correctly in a non-empty constructor when
    /// ForcePasswordReset and EmailVerified are explicitly set
    /// </summary>
    [Test]
    public void FullSetterTest()
    {
        var registeredUser = new RegisteredUser(1, "Brian", "McNamara", "brian@email.com", "123456abcdef", true, true);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(registeredUser.Id, Is.EqualTo(1));
            Assert.That(registeredUser.FirstName, Is.EqualTo("Brian"));
            Assert.That(registeredUser.LastName, Is.EqualTo("McNamara"));
            Assert.That(registeredUser.Email, Is.EqualTo("brian@email.com"));
            Assert.That(registeredUser.APIKey, Is.EqualTo("123456abcdef"));
            Assert.That(registeredUser.ForcePasswordReset, Is.True);
            Assert.That(registeredUser.EmailVerified, Is.True);
        }
    }
}
