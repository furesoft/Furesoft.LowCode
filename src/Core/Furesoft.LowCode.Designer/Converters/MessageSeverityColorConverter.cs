using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Furesoft.LowCode.Analyzing;

namespace Furesoft.LowCode.Designer.Converters;

public class MessageSeverityColorConverter : IValueConverter
{
    public static MessageSeverityColorConverter Instance = new();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Severity severity)
        {
            return severity switch
            {
                Severity.Warning => new SolidColorBrush(new Color(255, 217, 183, 41)),
                Severity.Error => Brushes.Red,
                Severity.Info => new SolidColorBrush(new Color(255, 255, 165, 0)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
