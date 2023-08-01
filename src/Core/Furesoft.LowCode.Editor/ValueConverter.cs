using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Furesoft.LowCode.Editor;

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

    protected abstract object Convert(TValue value, Type targetType, object parameter);

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public sealed override object ProvideValue(IServiceProvider serviceProvider)
    {
        return _instance;
    }
}
