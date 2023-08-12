using Avalonia.Media;
using Furesoft.LowCode.Analyzing;

namespace Furesoft.LowCode.Designer.Converters;

public class MessageSeverityColorConverter : ValueConverter<MessageSeverityColorConverter, Severity>
{
    protected override object Convert(Severity severity, Type targetType, object parameter)
    {
        return severity switch
        {
            Severity.Warning => new SolidColorBrush(new Color(255, 217, 183, 41)),
            Severity.Error => Brushes.Red,
            Severity.Info => new SolidColorBrush(new Color(255, 255, 165, 0)),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
