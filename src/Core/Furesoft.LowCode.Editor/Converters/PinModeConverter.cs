using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Editor.Converters;

public class PinModeConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new PinModeConverter();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not PinMode mode)
        {
            return Brushes.Black;
        }

        return mode switch
        {
            PinMode.Input => Brushes.Green,
            PinMode.Output => Brushes.Red,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
