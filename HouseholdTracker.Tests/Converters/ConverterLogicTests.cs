using HouseholdTracker.Core.Converters;

namespace HouseholdTracker.Tests.Converters;

/// <summary>
/// Tests for the ConverterLogic static helper class
/// </summary>
[TestFixture]
internal sealed class ConverterLogicTests
{
    #region "StringToBool"

    /// <summary>
    /// Ensure a non-empty string returns true
    /// </summary>
    [Test]
    public void StringToBool_NonEmptyString_ReturnsTrue()
    {
        var result = ConverterLogic.StringToBool("Hello");
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Ensure an empty string returns false
    /// </summary>
    [Test]
    public void StringToBool_EmptyString_ReturnsFalse()
    {
        var result = ConverterLogic.StringToBool(string.Empty);
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Ensure a null string returns false
    /// </summary>
    [Test]
    public void StringToBool_NullString_ReturnsFalse()
    {
        var result = ConverterLogic.StringToBool(null);
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Ensure a whitespace-only string returns true as it is not empty
    /// </summary>
    [Test]
    public void StringToBool_WhitespaceString_ReturnsTrue()
    {
        var result = ConverterLogic.StringToBool("   ");
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Ensure an error message string returns true
    /// </summary>
    [Test]
    public void StringToBool_ErrorMessage_ReturnsTrue()
    {
        var result = ConverterLogic.StringToBool("Email address is required.");
        Assert.That(result, Is.True);
    }

    #endregion

    #region "InvertBool"

    /// <summary>
    /// Ensure true returns false
    /// </summary>
    [Test]
    public void InvertBool_True_ReturnsFalse()
    {
        var result = ConverterLogic.InvertBool(true);
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Ensure false returns true
    /// </summary>
    [Test]
    public void InvertBool_False_ReturnsTrue()
    {
        var result = ConverterLogic.InvertBool(false);
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Ensure inverting twice returns the original value
    /// </summary>
    [Test]
    public void InvertBool_InvertedTwice_ReturnsOriginal()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ConverterLogic.InvertBool(ConverterLogic.InvertBool(true)), Is.True);
            Assert.That(ConverterLogic.InvertBool(ConverterLogic.InvertBool(false)), Is.False);
        }
    }

    #endregion
}
