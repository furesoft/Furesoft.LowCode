namespace Furesoft.LowCode.Designer.Converters;

public class ColumnWidthConverter : ValueConverter<ColumnWidthConverter, bool>
{
    protected override object Convert(bool flag, Type targetType, object parameter)
    {
        if (parameter is double width)
        {
            if (flag)
            {
                return new GridLength(width);
            }

            return new GridLength(0);
        }

        return AvaloniaProperty.UnsetValue;
    }
}
