using CommunityToolkit.Mvvm.ComponentModel;

namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class PinViewModel : IPin
{
    [ObservableProperty] private PinAlignment _alignment;
    [ObservableProperty] private bool _canConnectToMultiplePins;
    [ObservableProperty] private double _height;
    [ObservableProperty] private PinMode _mode;
    [ObservableProperty] private string _name;
    [ObservableProperty] private INode _parent;
    [ObservableProperty] private double _width;
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;

    public event EventHandler<PinCreatedEventArgs> Created;

    public event EventHandler<PinRemovedEventArgs> Removed;

    public event EventHandler<PinMovedEventArgs> Moved;

    public event EventHandler<PinSelectedEventArgs> Selected;

    public event EventHandler<PinDeselectedEventArgs> Deselected;

    public event EventHandler<PinResizedEventArgs> Resized;

    public event EventHandler<PinConnectedEventArgs> Connected;

    public event EventHandler<PinDisconnectedEventArgs> Disconnected;

    public virtual bool CanConnect()
    {
        return true;
    }

    public virtual bool CanDisconnect()
    {
        return true;
    }

    public void OnCreated()
    {
        Created?.Invoke(this, new(this));
    }

    public void OnRemoved()
    {
        Removed?.Invoke(this, new(this));
    }

    public void OnMoved()
    {
        Moved?.Invoke(this, new(this, _x, _y));
    }

    public void OnSelected()
    {
        Selected?.Invoke(this, new(this));
    }

    public void OnDeselected()
    {
        Deselected?.Invoke(this, new(this));
    }

    public void OnResized()
    {
        Resized?.Invoke(this, new(this, _x, _y, _width, _height));
    }

    public void OnConnected()
    {
        Connected?.Invoke(this, new(this));
    }

    public void OnDisconnected()
    {
        Disconnected?.Invoke(this, new(this));
    }
}
