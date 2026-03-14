using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the ItemGroup class
/// </summary>
[TestFixture]
internal sealed class ItemGroupTests
{
    /// <summary>
    /// Ensure the expected default values are used in an empty constructor
    /// </summary>
    [Test]
    public void DefaultConstructorTest()
    {
        var itemGroup = new ItemGroup();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(itemGroup.Id, Is.Zero);
            Assert.That(itemGroup.UserId, Is.Zero);
            Assert.That(itemGroup.Name, Is.EqualTo(string.Empty));
            Assert.That(itemGroup.DefaultIcon, Is.Zero);
            Assert.That(itemGroup.DefaultItemSize, Is.EqualTo(ItemSizes.Grams));
            Assert.That(itemGroup.DefaultExpiryVisible, Is.True);
            Assert.That(itemGroup.DefaultLowPercentageWarning, Is.EqualTo(25));
            Assert.That(itemGroup.Enabled, Is.True);
            Assert.That(itemGroup.OnlineMatched, Is.True);
            Assert.That(itemGroup.SortOrder, Is.Zero);
        }
    }

    /// <summary>
    /// Ensure the values are set correctly in a non-empty constructor
    /// </summary>
    [Test]
    public void DefaultSetterTest()
    {
        var itemGroup = new ItemGroup(1, 2, "default", 1, ItemSizes.ML, false, 10, false, false, 1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(itemGroup.Id, Is.EqualTo(1));
            Assert.That(itemGroup.UserId, Is.EqualTo(2));
            Assert.That(itemGroup.Name, Is.EqualTo("default"));
            Assert.That(itemGroup.DefaultIcon, Is.EqualTo(1));
            Assert.That(itemGroup.DefaultItemSize, Is.EqualTo(ItemSizes.ML));
            Assert.That(itemGroup.DefaultExpiryVisible, Is.False);
            Assert.That(itemGroup.DefaultLowPercentageWarning, Is.EqualTo(10));
            Assert.That(itemGroup.Enabled, Is.False);
            Assert.That(itemGroup.OnlineMatched, Is.False);
            Assert.That(itemGroup.SortOrder, Is.EqualTo(1));
        }
    }
}
