using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Editor.Converters;

public class PinNameConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new PinNameConverter();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is PinViewModel vm)
        {
            return vm.Mode + ": " + vm.Name;
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
