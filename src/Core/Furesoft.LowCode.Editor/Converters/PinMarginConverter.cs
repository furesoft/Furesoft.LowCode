namespace Furesoft.LowCode.Editor.Converters;

public class PinMarginConverter : ValueConverter<PinMarginConverter, IPin>
{
    protected override object Convert(IPin pin, Type targetType, object parameter)
    {
        if (pin is not null)
        {
            return new Thickness(-pin.Width / 2, -pin.Height / 2, 0, 0);
        }

        return new Thickness(0);
    }
}
