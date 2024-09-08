namespace Furesoft.LowCode.Editor.Model;

public sealed class PinCreatedEventArgs(IPin pin) : EventArgs
{
    public IPin Pin { get; } = pin;
}

public sealed class PinRemovedEventArgs(IPin pin) : EventArgs
{
    public IPin Pin { get; } = pin;
}

public sealed class PinMovedEventArgs(IPin pin, double x, double y) : EventArgs
{
    public IPin Pin { get; } = pin;

    public double X { get; } = x;

    public double Y { get; } = y;
}

public sealed class PinSelectedEventArgs(IPin pin) : EventArgs
{
    public IPin Pin { get; } = pin;
}

public class PinDeselectedEventArgs(IPin pin) : EventArgs
{
    public IPin Pin { get; } = pin;
}

public sealed class PinResizedEventArgs(IPin pin, double x, double y, double width, double height)
    : EventArgs
{
    public IPin Pin { get; } = pin;

    public double X { get; } = x;

    public double Y { get; } = y;

    public double Width { get; } = width;

    public double Height { get; } = height;
}

public sealed class PinConnectedEventArgs(IPin pin) : EventArgs
{
    public IPin Pin { get; } = pin;
}

public sealed class PinDisconnectedEventArgs(IPin pin) : EventArgs
{
    public IPin Pin { get; } = pin;
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
