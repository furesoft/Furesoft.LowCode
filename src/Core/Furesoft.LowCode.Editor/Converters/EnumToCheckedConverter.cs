using System.Globalization;
using Avalonia.Data;

namespace Furesoft.LowCode.Editor.Converters;

internal class EnumToCheckedConverter : ValueConverter<EnumToCheckedConverter, bool>
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter);
    }

    protected override object Convert(bool isChecked, Type targetType, object parameter)
    {
        return isChecked ? parameter : BindingOperations.DoNothing;
    }
}
