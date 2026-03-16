using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the UserPreferences class
/// </summary>
[TestFixture]
internal sealed class UserPreferencesTests
{
    /// <summary>
    /// Ensure the expected default values are used in an empty constructor
    /// </summary>
    [Test]
    public void DefaultConstructorTest()
    {
        var userPreferences = new UserPreferences();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(userPreferences.Id, Is.Zero);
            Assert.That(userPreferences.UserId, Is.Zero);
            Assert.That(userPreferences.Preference, Is.EqualTo(Preferences.None));
            Assert.That(userPreferences.Value, Is.False);
        }
    }

    /// <summary>
    /// Ensure the values are set correctly in a non-empty constructor
    /// </summary>
    [Test]
    public void DefaultSetterTest()
    {
        var userPreferences = new UserPreferences(1, 2, Preferences.Marketing, true);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(userPreferences.Id, Is.EqualTo(1));
            Assert.That(userPreferences.UserId, Is.EqualTo(2));
            Assert.That(userPreferences.Preference, Is.EqualTo(Preferences.Marketing));
            Assert.That(userPreferences.Value, Is.True);
        }
    }
}
