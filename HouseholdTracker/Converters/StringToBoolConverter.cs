using System.Globalization;
using HouseholdTracker.Core.Converters;

namespace HouseholdTracker.Converters;

/// <summary>
/// Converts a string to a boolean — returns true if the string is not null or empty.
/// Used to control visibility of error labels.
/// </summary>
public class StringToBoolConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => ConverterLogic.StringToBool(value as string);

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
