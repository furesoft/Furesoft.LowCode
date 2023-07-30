using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Furesoft.LowCode.Editor.Converters;

internal class EnumToCheckedConverter : IValueConverter
{
    public static readonly EnumToCheckedConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked)
        {
            return parameter;
        }

        return BindingOperations.DoNothing;
    }
}
