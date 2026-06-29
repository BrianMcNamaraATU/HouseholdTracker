using System.Globalization;
using HouseholdTracker.Core.Converters;

namespace HouseholdTracker.Converters;

/// <summary>
/// Converts a boolean to its inverse.
/// Used to hide elements when a condition is true.
/// </summary>
public class InvertedBoolConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => ConverterLogic.InvertBool(value is bool b && b);

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => ConverterLogic.InvertBool(value is bool b && b);
}
