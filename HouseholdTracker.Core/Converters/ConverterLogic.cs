namespace HouseholdTracker.Core.Converters;

/// <summary>
/// Static helper methods containing the logic for value converters.
/// Kept in Core so they can be tested without a MAUI dependency.
/// </summary>
public static class ConverterLogic
{
    /// <summary>
    /// Returns true if the string is not null or empty
    /// </summary>
    /// <param name="value">The string to check</param>
    /// <returns>True if the string has content</returns>
    public static bool StringToBool(string? value)
        => !string.IsNullOrEmpty(value);

    /// <summary>
    /// Returns the inverse of a boolean value
    /// </summary>
    /// <param name="value">The boolean to invert</param>
    /// <returns>The inverted boolean</returns>
    public static bool InvertBool(bool value)
        => !value;
}
