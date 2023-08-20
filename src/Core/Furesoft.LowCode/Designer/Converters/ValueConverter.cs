using System.Globalization;
using Avalonia.Data.Converters;

namespace Furesoft.LowCode.Designer.Converters;

public abstract class ValueConverter<TValueConverter, TValue> : MarkupExtension, IValueConverter
    where TValueConverter : ValueConverter<TValueConverter, TValue>, new()
{
    private static readonly TValueConverter _instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TValue tValue)
        {
            return default;
        }

        return Convert(tValue, targetType, parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ConvertBack((TValue)value, targetType, parameter);
    }

    public virtual object ConvertBack(TValue value, Type targetType, object parameter)
    {
        throw new NotImplementedException();
    }

    protected abstract object Convert(TValue value, Type targetType, object parameter);

    public sealed override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _instance;
    }
}
