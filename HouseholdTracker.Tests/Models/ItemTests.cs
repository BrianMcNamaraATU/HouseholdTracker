using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the Item class
/// </summary>
[TestFixture]
internal sealed class ItemTests
{
    /// <summary>
    /// Ensure the expected default values are used in an empty constructor
    /// </summary>
    [Test]
    public void DefaultConstructorTest()
    {
        var item = new Item();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(item.Id, Is.Zero);
            Assert.That(item.UserId, Is.Zero);
            Assert.That(item.Name, Is.EqualTo(string.Empty));
            Assert.That(item.Icon, Is.Zero);
            Assert.That(item.ExpiryVisible, Is.True);
            Assert.That(item.MaxSize, Is.Zero);
            Assert.That(item.ItemSize, Is.EqualTo(ItemSizes.Grams));
            Assert.That(item.CurrentLevelAsPercentage, Is.Zero);
            Assert.That(item.LowPercentageWarning, Is.Zero);
            Assert.That(item.CurrentExpiry, Is.EqualTo(DateTime.MinValue));
            Assert.That(item.LastUpdateUTC, Is.EqualTo(DateTime.MinValue));
            Assert.That(item.Enabled, Is.True);
            Assert.That(item.SortOrder, Is.Zero);
        }
    }

    /// <summary>
    /// Ensure the values are set correctly in a non-empty constructor
    /// </summary>
    [Test]
    public void DefaultSetterTest()
    {
        var item = new Item(1, 2, "default", 1, false, 100, ItemSizes.ML, 1, 1, DateTime.MaxValue, DateTime.MaxValue, false, 1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(item.Id, Is.EqualTo(1));
            Assert.That(item.UserId, Is.EqualTo(2));
            Assert.That(item.Name, Is.EqualTo("default"));
            Assert.That(item.Icon, Is.EqualTo(1));
            Assert.That(item.ExpiryVisible, Is.False);
            Assert.That(item.MaxSize, Is.EqualTo(100));
            Assert.That(item.ItemSize, Is.EqualTo(ItemSizes.ML));
            Assert.That(item.CurrentLevelAsPercentage, Is.EqualTo(1));
            Assert.That(item.LowPercentageWarning, Is.EqualTo(1));
            Assert.That(item.CurrentExpiry, Is.EqualTo(DateTime.MaxValue));
            Assert.That(item.LastUpdateUTC, Is.EqualTo(DateTime.MaxValue));
            Assert.That(item.Enabled, Is.False);
            Assert.That(item.SortOrder, Is.EqualTo(1));
        }
    }
}
