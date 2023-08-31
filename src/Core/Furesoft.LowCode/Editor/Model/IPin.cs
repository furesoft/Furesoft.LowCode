namespace Furesoft.LowCode.Editor.Model;

public sealed class PinCreatedEventArgs : EventArgs
{
    public PinCreatedEventArgs(IPin pin)
    {
        Pin = pin;
    }

    public IPin Pin { get; }
}

public sealed class PinRemovedEventArgs : EventArgs
{
    public PinRemovedEventArgs(IPin pin)
    {
        Pin = pin;
    }

    public IPin Pin { get; }
}

public sealed class PinMovedEventArgs : EventArgs
{
    public PinMovedEventArgs(IPin pin, double x, double y)
    {
        X = x;
        Y = y;
        Pin = pin;
    }

    public IPin Pin { get; }

    public double X { get; }

    public double Y { get; }
}

public sealed class PinSelectedEventArgs : EventArgs
{
    public PinSelectedEventArgs(IPin pin)
    {
        Pin = pin;
    }

    public IPin Pin { get; }
}

public class PinDeselectedEventArgs : EventArgs
{
    public PinDeselectedEventArgs(IPin pin)
    {
        Pin = pin;
    }

    public IPin Pin { get; }
}

public sealed class PinResizedEventArgs : EventArgs
{
    public PinResizedEventArgs(IPin pin, double x, double y, double width, double height)
    {
        Pin = pin;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public IPin Pin { get; }

    public double X { get; }

    public double Y { get; }

    public double Width { get; }

    public double Height { get; }
}

public sealed class PinConnectedEventArgs : EventArgs
{
    public PinConnectedEventArgs(IPin pin)
    {
        Pin = pin;
    }

    public IPin Pin { get; }
}

public sealed class PinDisconnectedEventArgs : EventArgs
{
    public PinDisconnectedEventArgs(IPin pin)
    {
        Pin = pin;
    }

    public IPin Pin { get; }
}

public interface IPin
{
    string Name { get; set; }
    INode Parent { get; set; }
    double X { get; set; }
    double Y { get; set; }
    double Width { get; set; }
    double Height { get; set; }
    PinAlignment Alignment { get; set; }
    PinMode Mode { get; set; }
    bool CanConnectToMultiplePins { get; set; }
    bool CanConnect();
    bool CanDisconnect();
    event EventHandler<PinCreatedEventArgs> Created;
    event EventHandler<PinRemovedEventArgs> Removed;
    event EventHandler<PinMovedEventArgs> Moved;
    event EventHandler<PinSelectedEventArgs> Selected;
    event EventHandler<PinDeselectedEventArgs> Deselected;
    event EventHandler<PinResizedEventArgs> Resized;
    event EventHandler<PinConnectedEventArgs> Connected;
    event EventHandler<PinDisconnectedEventArgs> Disconnected;
    void OnCreated();
    void OnRemoved();
    void OnMoved();
    void OnSelected();
    void OnDeselected();
    void OnResized();
    void OnConnected();
    void OnDisconnected();
}
