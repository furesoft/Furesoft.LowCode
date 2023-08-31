namespace Furesoft.LowCode.Editor.Converters;

public class PinToPointConverter : ValueConverter<PinToPointConverter, IPin>
{
    protected override object Convert(IPin pin, Type targetType, object parameter)
    {
        var x = pin.X;
        var y = pin.Y;

        if (pin.Parent is not null)
        {
            x += pin.Parent.X;
            y += pin.Parent.Y;
        }

        return new Point(x, y);
    }
}
