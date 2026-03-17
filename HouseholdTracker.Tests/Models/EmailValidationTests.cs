using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the EmailValidation class
/// </summary>
[TestFixture]
internal sealed class EmailValidationTests
{
    /// <summary>
    /// Ensure the expected default values are used in an empty constructor
    /// </summary>
    [Test]
    public void DefaultConstructorTest()
    {
        var emailValidation = new EmailValidation();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emailValidation.Id, Is.Zero);
            Assert.That(emailValidation.UserId, Is.Zero);
            Assert.That(emailValidation.PreviousEmail, Is.EqualTo(string.Empty));
            Assert.That(emailValidation.NewEmail, Is.EqualTo(string.Empty));
            Assert.That(emailValidation.EmailCode, Is.EqualTo(string.Empty));
            Assert.That(emailValidation.SentUTC, Is.EqualTo(DateTime.MinValue));
            Assert.That(emailValidation.ValidatedUTC, Is.Null);
        }
    }

    /// <summary>
    /// Ensure the values are set correctly in a non-empty constructor
    /// </summary>
    [Test]
    public void DefaultSetterTest()
    {
        var emailValidation = new EmailValidation(1, 2, "previous@email.com", "new@email.com", "ABC123", DateTime.MaxValue, DateTime.MinValue);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(emailValidation.Id, Is.EqualTo(1));
            Assert.That(emailValidation.UserId, Is.EqualTo(2));
            Assert.That(emailValidation.PreviousEmail, Is.EqualTo("previous@email.com"));
            Assert.That(emailValidation.NewEmail, Is.EqualTo("new@email.com"));
            Assert.That(emailValidation.EmailCode, Is.EqualTo("ABC123"));
            Assert.That(emailValidation.SentUTC, Is.EqualTo(DateTime.MaxValue));
            Assert.That(emailValidation.ValidatedUTC, Is.EqualTo(DateTime.MinValue));
        }
    }
}
