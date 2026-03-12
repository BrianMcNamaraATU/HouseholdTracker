using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// Tests for the LoggedInUser Service
/// </summary>
public class LoggedInUserServiceTests
{
    private RegisteredUser _user1;
    private RegisteredUser _user2;

    /// <summary>
    /// Create a new instance of the LoggedInUserService using the Test Storage configuration
    /// for each test. Also create 2 Registered Users
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        LoggedInUserService.Initialize(new TestStorageLoggedInUserService());

        _user1 = new RegisteredUser
        {
            Id = 1,
            Username = "testuser1",
            FirstName = "Test",
            Password = "testpassword1",
            APIKey = "apikey-1"
        };

        _user2 = new RegisteredUser
        {
            Id = 2,
            Username = "testuser2",
            FirstName = "Test2",
            Password = "testpassword2",
            APIKey = "apikey-2"
        };
    }

    /// <summary>
    /// Login as 2 users and ensure that the correct values are stored
    /// </summary>
    [Test]
    public async Task LoginUser_StoresValues()
    {
        await LoggedInUserService.LoginUserAsync(_user1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(LoggedInUserService.LoggedInUserId, Is.EqualTo(1));
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.EqualTo("Test"));
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.EqualTo("apikey-1"));
        }

        await LoggedInUserService.LoginUserAsync(_user2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(LoggedInUserService.LoggedInUserId, Is.EqualTo(2));
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.EqualTo("Test2"));
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.EqualTo("apikey-2"));
        }
    }

    /// <summary>
    /// Ensure that when a user Logs out that the values are cleared, and that it is then
    /// possible to login again
    /// </summary>
    [Test]
    public async Task LogoutUser_ClearsValues()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(LoggedInUserService.LoggedInUserId, Is.Null);
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.Null.Or.Empty);
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.Null.Or.Empty);
        }

        await LoggedInUserService.LoginUserAsync(_user1);
        await LoggedInUserService.LogoutUserAsync();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(LoggedInUserService.LoggedInUserId, Is.Null);
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.Null.Or.Empty);
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.Null.Or.Empty);
        }

        await LoggedInUserService.LoginUserAsync(_user2);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(LoggedInUserService.LoggedInUserId, Is.EqualTo(2));
            Assert.That(await LoggedInUserService.GetLoggedInUserFirstNameAsync(), Is.EqualTo("Test2"));
            Assert.That(await LoggedInUserService.GetLoggedInUserAPIKeyAsync(), Is.EqualTo("apikey-2"));
        }
    }
}
