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

    private DateTime _currentDate;
    private DateTime _currentDateTime;

    private ItemGroup _itemGroup1;
    private ItemGroup _itemGroup2;
    private ItemGroup _itemGroup3;

    private Item _item1;
    private Item _item2;
    private Item _item3;

    private UserPreferences _pref1;
    private UserPreferences _pref2;

    /// <summary>
    /// Create a new entity of the LocalDatabaseService for each test
    /// </summary>
    [SetUp]
    public async Task SetUp()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.db");
        _service = new LocalDatabaseService(_dbPath);
        await _service.InitAsync();

        _currentDate = DateTime.Today;
        _currentDateTime = DateTime.UtcNow;

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

        _item1 = new Item
        {
            Id = 1,
            UserId = 1,
            Name = "Item1",
            Icon = 1,
            ExpiryVisible = true,
            MaxSize = 100,
            ItemSize = ItemSizes.ML,
            CurrentLevelAsPercentage = 80,
            CurrentExpiry = _currentDate.AddDays(1),
            LastUpdateUTC = _currentDateTime,
            Enabled = true,
            SortOrder = 1
        };

        _item2 = new Item
        {
            Id = 2,
            UserId = 1,
            Name = "Item2",
            Icon = 2,
            ExpiryVisible = true,
            MaxSize = 100,
            ItemSize = ItemSizes.ML,
            CurrentLevelAsPercentage = 70,
            CurrentExpiry = _currentDate.AddDays(2),
            LastUpdateUTC = _currentDateTime.AddDays(-1),
            Enabled = true,
            SortOrder = 2
        };

        _item3 = new Item
        {
            Id = 3,
            UserId = 2,
            Name = "Item3",
            Icon = 1,
            ExpiryVisible = true,
            MaxSize = 100,
            ItemSize = ItemSizes.Grams,
            CurrentLevelAsPercentage = 90,
            CurrentExpiry = _currentDate,
            LastUpdateUTC = _currentDateTime.AddHours(-1),
            Enabled = false,
            SortOrder = 1
        };

        _pref1 = new UserPreferences
        {
            Id = 1,
            UserId = 1,
            Preference = Preferences.Marketing,
            Value = true
        };

        _pref2 = new UserPreferences
        {
            Id = 2,
            UserId = 1,
            Preference = Preferences.SharedAccountAcceptance,
            Value = false
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
    /// Test to check if inserting 3 Item Groups functions correctly
    /// For success the total count should be 3, and then removing one
    /// giving a new total of 2
    /// </summary>
    [Test]
    public async Task AddMultipleItemGroups_ShouldInsertSuccessfully()
    {
        var result1 = await _service.AddAsync(_itemGroup1);
        var result2 = await _service.AddAsync(_itemGroup2);
        await _service.AddAsync(_itemGroup3);

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
        await _service.DeleteAsync(_itemGroup1);
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
        await _service.AddAsync(_itemGroup1);
        await _service.AddAsync(_itemGroup2);
        var result1 = await _service.GetItemGroupCountAsync();

        await _service.DeleteItemGroupsAllAsync();
        var result2 = await _service.GetItemGroupCountAsync();

        await _service.AddAsync(_itemGroup1);
        await _service.AddAsync(_itemGroup2);
        await _service.AddAsync(_itemGroup3);
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
        await _service.AddAsync(_itemGroup1);
        await _service.AddAsync(_itemGroup2);
        await _service.AddAsync(_itemGroup3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _service.GetItemGroupsAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemGroupsActiveAsync(), Has.Count.EqualTo(2));
        }

        _itemGroup1.Enabled = false;
        await _service.UpdateAsync(_itemGroup1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _service.GetItemGroupsAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemGroupsActiveAsync(), Has.Count.EqualTo(1));
        }
    }
    #endregion

    #region "Item"
    /// <summary>
    /// Test to check if inserting 3 Items functions correctly
    /// For success the total count should be 3
    /// And then remove one, giving a total of 2
    /// </summary>
    [Test]
    public async Task AddMultipleItemsRemove1_ShouldInsertSuccessfully()
    {
        var result1 = await _service.AddAsync(_item1);
        var result2 = await _service.AddAsync(_item2);
        await _service.AddAsync(_item3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(1));
            Assert.That(_item1.Id, Is.EqualTo(1));
            Assert.That(result2, Is.EqualTo(1));
            Assert.That(_item2.Id, Is.EqualTo(2));
            Assert.That(await _service.GetItemCountAsync(), Is.EqualTo(3));
            Assert.That(await _service.GetItemAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemActiveAsync(), Has.Count.EqualTo(2));
        }

        // Remove one of the Items
        await _service.DeleteAsync(_item1);
        Assert.That(await _service.GetItemCountAsync(), Is.EqualTo(2));
    }

    /// <summary>
    /// Test insertions, followed by deletions, followed by further insertions works
    /// It will work if the same Id can be inserted after the database has had all its
    /// previous Item deleted
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task InsertionDeletionInsertionItemTest()
    {
        await _service.AddAsync(_item1);
        await _service.AddAsync(_item2);
        var result1 = await _service.GetItemCountAsync();

        await _service.DeleteItemAllAsync();
        var result2 = await _service.GetItemCountAsync();

        await _service.AddAsync(_item1);
        await _service.AddAsync(_item2);
        await _service.AddAsync(_item3);
        var result3 = await _service.GetItemCountAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(2));
            Assert.That(result2, Is.Zero);
            Assert.That(result3, Is.EqualTo(3));
        }
    }

    /// <summary>
    /// Test updating the Items in the LocalDatabaseService
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task UpdateitemTest_EnabledStatusChanges()
    {
        await _service.AddAsync(_item1);
        await _service.AddAsync(_item2);
        await _service.AddAsync(_item3);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _service.GetItemAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemActiveAsync(), Has.Count.EqualTo(2));
        }

        _item1.Enabled = false;
        await _service.UpdateAsync(_item1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _service.GetItemAsync(), Has.Count.EqualTo(3));
            Assert.That(await _service.GetItemActiveAsync(), Has.Count.EqualTo(1));
        }
    }
    #endregion

    #region "UserPreferences"
    /// <summary>
    /// Test to check if inserting 2 UserPreferences functions correctly
    /// For success the total count should be 2
    /// And then remove one, giving a total of 1
    /// </summary>
    [Test]
    public async Task AddMultipleUserPreferencesRemove1_ShouldInsertSuccessfully()
    {
        var result1 = await _service.AddAsync(_pref1);
        var result2 = await _service.AddAsync(_pref2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(1));
            Assert.That(_item1.Id, Is.EqualTo(1));
            Assert.That(result2, Is.EqualTo(1));
            Assert.That(_item2.Id, Is.EqualTo(2));
            Assert.That(await _service.GetUserPreferencesCountAsync(), Is.EqualTo(2));
            Assert.That(await _service.GetUserPreferencesAsync(), Has.Count.EqualTo(2));
        }

        // Remove one of the Items
        await _service.DeleteAsync(_pref1);
        Assert.That(await _service.GetUserPreferencesCountAsync(), Is.EqualTo(1));
    }

    /// <summary>
    /// Test insertions, followed by deletions, followed by further insertions works
    /// It will work if the same Id can be inserted after the database has had all its
    /// previous Item deleted
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task InsertionDeletionInsertionUserPreferencesTest()
    {
        await _service.AddAsync(_pref1);
        await _service.AddAsync(_pref2);
        var result1 = await _service.GetUserPreferencesCountAsync();

        await _service.DeleteUserPreferencesAllAsync();
        var result2 = await _service.GetUserPreferencesCountAsync();

        await _service.AddAsync(_pref1);
        await _service.AddAsync(_pref2);
        var result3 = await _service.GetUserPreferencesCountAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(2));
            Assert.That(result2, Is.Zero);
            Assert.That(result3, Is.EqualTo(2));
        }
    }

    /// <summary>
    /// Test updating the UserPreferences in the LocalDatabaseService
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task UpdateUserPreferenceTest_EnabledStatusChanges()
    {
        await _service.AddAsync(_pref1);
        await _service.AddAsync(_pref2);

        var prefs = await _service.GetUserPreferencesAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(prefs, Has.Count.EqualTo(2));
            Assert.That(prefs.First(up => up.Id == 1).Value, Is.True);
        }

        _pref1.Value = false;
        await _service.UpdateAsync(_pref1);

        prefs = await _service.GetUserPreferencesAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(prefs, Has.Count.EqualTo(2));
            Assert.That(prefs.First(up => up.Id == 1).Value, Is.False);
        }
    }
    #endregion
}
