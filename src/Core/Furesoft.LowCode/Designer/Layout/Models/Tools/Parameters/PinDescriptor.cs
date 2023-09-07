namespace Furesoft.LowCode.Designer.Layout.Models.Tools.Parameters;

public partial class PinDescriptor : ObservableObject
{
    [ObservableProperty] private PinMode _direction;
    [ObservableProperty] private PinAlignment _alignment;
    [ObservableProperty] private string _name;
    [ObservableProperty] private bool _multipleConnections;

    public PinDescriptor(string name, PinMode direction, PinAlignment alignment, bool multipleConnections)
    {
        _name = name;
        _direction = direction;
        _alignment = alignment;
        _multipleConnections = multipleConnections;
    }

    public PinDescriptor()
    {

    }
}
