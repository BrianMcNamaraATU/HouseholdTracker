using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// Tests for the Local Database Service
/// </summary>
public class LocalDatabaseServiceTests
{
    private LocalDatabaseService _service;
    private string _dbPath;

    private ItemGroup _itemGroup1;
    private ItemGroup _itemGroup2;
    private ItemGroup _itemGroup3;

    /// <summary>
    /// Create a new entity of the LocalDatabaseService for each test
    /// </summary>
    [SetUp]
    public async Task SetUp()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.db");
        _service = new LocalDatabaseService(_dbPath);
        await _service.InitAsync();

        _itemGroup1 = new ItemGroup
        {
            Id = 1,
            UserId = 1,
            Name = "Group1",
            DefaultIcon = 1,
            DefaultItemSize = ItemSizes.Grams,
            DefaultExpiryVisible = true,
            DefaultLowPercentageWarning = 25,
            Enabled = true,
            OnlineMatched = true,
            SortOrder = 1
        };

        _itemGroup2 = new ItemGroup
        {
            Id = 2,
            UserId = 1,
            Name = "Group2",
            DefaultIcon = 2,
            DefaultItemSize = ItemSizes.Grams,
            DefaultExpiryVisible = false,
            DefaultLowPercentageWarning = 10,
            Enabled = true,
            OnlineMatched = true,
            SortOrder = 2
        };

        _itemGroup3 = new ItemGroup
        {
            Id = 3,
            UserId = 1,
            Name = "Group3",
            DefaultIcon = 1,
            DefaultItemSize = ItemSizes.ML,
            DefaultExpiryVisible = true,
            DefaultLowPercentageWarning = 15,
            Enabled = false,
            OnlineMatched = true,
            SortOrder = 3
        };
    }

    /// <summary>
    /// Remove the LocalDatabaseService entity after each test
    /// </summary>
    [TearDown]
    public async Task TearDown()
    {
        await _service.DisposeAsync();

        if (File.Exists(_dbPath))
            File.Delete(_dbPath);
    }

    #region "ItemGroup"
    /// <summary>
    /// Test to check if inserting 2 Item Groups functions correctly
    /// For success the total count should be 2
    /// </summary>
    [Test]
    public async Task AddMultipleItemGroups_ShouldInsertSuccessfully()
    {
        var result1 = await _service.AddItemGroupAsync(_itemGroup1);
        var result2 = await _service.AddItemGroupAsync(_itemGroup2);
        await _service.AddItemGroupAsync(_itemGroup3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(1));
            Assert.That(_itemGroup1.Id, Is.EqualTo(1));
            Assert.That(result2, Is.EqualTo(1));
            Assert.That(_itemGroup2.Id, Is.EqualTo(2));
            Assert.That(await _service.GetItemGroupCountAsync(), Is.EqualTo(3));
            Assert.That(await _service.GetItemGroupsAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemGroupsActiveAsync(), Has.Count.EqualTo(2));
        }

        // Remove one of the ItemGroups
        await _service.DeleteItemGroupAsync(_itemGroup1);
        Assert.That(await _service.GetItemGroupCountAsync(), Is.EqualTo(2));
    }

    /// <summary>
    /// Test insertions, followed by deletions, followed by further insertions works
    /// It will work if the same Id can be inserted after the database has had all its
    /// previous ItemGroups deleted
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task InsertionDeletionInsertionTest()
    {
        await _service.AddItemGroupAsync(_itemGroup1);
        await _service.AddItemGroupAsync(_itemGroup2);
        var result1 = await _service.GetItemGroupCountAsync();

        await _service.DeleteItemGroupsAllAsync();
        var result2 = await _service.GetItemGroupCountAsync();

        await _service.AddItemGroupAsync(_itemGroup1);
        await _service.AddItemGroupAsync(_itemGroup2);
        await _service.AddItemGroupAsync(_itemGroup3);
        var result3 = await _service.GetItemGroupCountAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(2));
            Assert.That(result2, Is.Zero);
            Assert.That(result3, Is.EqualTo(3));
        }
    }

    /// <summary>
    /// Test updating the ItemGroups in the LocalDatabaseService
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task UpdateTest_EnabledStatusChanges()
    {
        await _service.AddItemGroupAsync(_itemGroup1);
        await _service.AddItemGroupAsync(_itemGroup2);
        await _service.AddItemGroupAsync(_itemGroup3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _service.GetItemGroupsAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemGroupsActiveAsync(), Has.Count.EqualTo(2));
        }

        _itemGroup1.Enabled = false;
        await _service.UpdateItemGroupAsync(_itemGroup1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _service.GetItemGroupsAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemGroupsActiveAsync(), Has.Count.EqualTo(1));
        }
    }
    #endregion
}
