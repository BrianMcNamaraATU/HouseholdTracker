using HouseholdTracker.Core.Models;

namespace HouseholdTracker.Tests.Models;

/// <summary>
/// Tests for the ApiResult class
/// </summary>
[TestFixture]
internal sealed class ApiResultTests
{
    /// <summary>
    /// Ensure Ok sets Success to true and Data correctly
    /// </summary>
    [Test]
    public void Ok_SetsSuccessToTrue()
    {
        var result = ApiResult<string>.Ok("test data");
        Assert.That(result.Success, Is.True);
    }

    /// <summary>
    /// Ensure Ok sets Data correctly
    /// </summary>
    [Test]
    public void Ok_SetsDataCorrectly()
    {
        var result = ApiResult<string>.Ok("test data");
        Assert.That(result.Data, Is.EqualTo("test data"));
    }

    /// <summary>
    /// Ensure Ok sets ErrorMessage to null
    /// </summary>
    [Test]
    public void Ok_SetsErrorMessageToNull()
    {
        var result = ApiResult<string>.Ok("test data");
        Assert.That(result.ErrorMessage, Is.Null);
    }

    /// <summary>
    /// Ensure Fail sets Success to false
    /// </summary>
    [Test]
    public void Fail_SetsSuccessToFalse()
    {
        var result = ApiResult<string>.Fail("Something went wrong.");
        Assert.That(result.Success, Is.False);
    }

    /// <summary>
    /// Ensure Fail sets ErrorMessage correctly
    /// </summary>
    [Test]
    public void Fail_SetsErrorMessageCorrectly()
    {
        var result = ApiResult<string>.Fail("Something went wrong.");
        Assert.That(result.ErrorMessage, Is.EqualTo("Something went wrong."));
    }

    /// <summary>
    /// Ensure Fail sets Data to null
    /// </summary>
    [Test]
    public void Fail_SetsDataToNull()
    {
        var result = ApiResult<string>.Fail("Something went wrong.");
        Assert.That(result.Data, Is.Null);
    }

    /// <summary>
    /// Ensure Ok works correctly with a RegisteredUser type
    /// </summary>
    [Test]
    public void Ok_WithRegisteredUser_SetsDataCorrectly()
    {
        var user = new RegisteredUser(1, "Brian", "Smith", "brian@example.com", "apikey-1");
        var result = ApiResult<RegisteredUser>.Ok(user);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.EqualTo(user));
            Assert.That(result.ErrorMessage, Is.Null);
        }
    }

    /// <summary>
    /// Ensure Ok works correctly with a bool type
    /// </summary>
    [Test]
    public void Ok_WithBool_SetsDataCorrectly()
    {
        var result = ApiResult<bool>.Ok(true);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Data, Is.True);
        }
    }
}
