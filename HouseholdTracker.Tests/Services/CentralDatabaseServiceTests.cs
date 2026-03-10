using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// Tests for the Central Database Service
/// </summary>
public class CentralDatabaseServiceTests
{
    private CentralDatabaseService _service;
    private string _dbPath;

    private RegisteredUser _user1;
    private RegisteredUser _user2;

    /// <summary>
    /// Create a new entity of the CentralDatabaseService for each test
    /// </summary>
    [SetUp]
    public async Task SetUp()
    {
        _dbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.db");
        _service = new CentralDatabaseService(_dbPath);
        await _service.InitAsync();

        _user1 = new RegisteredUser
        {
            Username = "testuser1",
            Password = "testpassword1",
            APIKey = "apikey-1"
        };

        _user2 = new RegisteredUser
        {
            Username = "testuser2",
            Password = "testpassword2",
            APIKey = "apikey-2"
        };
    }

    /// <summary>
    /// Remove the CentralDatabaseService entity after each test
    /// </summary>
    [TearDown]
    public async Task TearDown()
    {
        await _service.DisposeAsync();

        if (File.Exists(_dbPath))
            File.Delete(_dbPath);
    }

    #region "RegisteredUser"
    /// <summary>
    /// Test to check if inserting 2 Registered Users functions correctly
    /// For success they should have incremental Id's and the total count should be 2
    /// </summary>
    [Test]
    public async Task AddMultipleRegisteredUsers_ShouldInsertSuccessfully()
    {
        var result1 = await _service.AddRegisteredUserAsync(_user1);
        var result2 = await _service.AddRegisteredUserAsync(_user2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(1));
            Assert.That(_user1.Id, Is.EqualTo(1));
            Assert.That(result2, Is.EqualTo(1));
            Assert.That(_user2.Id, Is.EqualTo(2));
            Assert.That(await _service.GetRegisteredUserCountAsync(), Is.EqualTo(2));
        }
    }

    /// <summary>
    /// Test the UserIsValid Method using Id/APIKey credentials
    /// The first should return true and the second false
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task UserIsValid_WithIdAndAPIKeyCredentials_TestBothResults()
    {
        await _service.AddRegisteredUserAsync(_user1);
        await _service.AddRegisteredUserAsync(_user2);

        var result1 = await _service.UserIsValid(_user1.Id, "apikey-1");
        var result2 = await _service.UserIsValid(_user2.Id, "apikey-1");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.True);
            Assert.That(result2, Is.False);
        }
    }

    /// <summary>
    /// Test the UserIsValid method using username/password credentials
    /// The first should return a valid user and the second null
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task UserIsValid_WithUsernamePasswordCredentials_TestBothResults()
    {
        await _service.AddRegisteredUserAsync(_user1);
        await _service.AddRegisteredUserAsync(_user2);

        var result1 = await _service.UserIsValid("testuser1", "testpassword1");
        var result2 = await _service.UserIsValid("testuser2", "testpassword1");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.Not.Null);
            Assert.That(result1.Id, Is.EqualTo(1));
            Assert.That(result2, Is.Null);
        }
    }

    /// <summary>
    /// Tests to check if the Update Registered User is functioning correctly
    /// It should only be possible to return the User with the new credentials after the update
    /// </summary>
    [Test]
    public async Task UpdateRegisteredUser_ShouldUpdateSuccessfully()
    {
        await _service.AddRegisteredUserAsync(_user1);

        _user1.Password = "newpassword1";
        var result1 = await _service.UpdateRegisteredUserAsync(_user1);
        var updated1 = await _service.UserIsValid("testuser1", "newpassword1");
        var updated2 = await _service.UserIsValid("testuser1", "password1");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.EqualTo(1));
            Assert.That(updated1, Is.Not.Null);
            Assert.That(updated2, Is.Null);
        }
    }
    #endregion
}
