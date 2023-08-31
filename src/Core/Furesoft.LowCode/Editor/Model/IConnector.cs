namespace Furesoft.LowCode.Editor.Model;

public sealed class ConnectorCreatedEventArgs : EventArgs
{
    public ConnectorCreatedEventArgs(IConnector connector)
    {
        Connector = connector;
    }

    public IConnector Connector { get; }
}

public sealed class ConnectorRemovedEventArgs : EventArgs
{
    public ConnectorRemovedEventArgs(IConnector connector)
    {
        Connector = connector;
    }

    public IConnector Connector { get; }
}

public sealed class ConnectorSelectedEventArgs : EventArgs
{
    public ConnectorSelectedEventArgs(IConnector connector)
    {
        Connector = connector;
    }

    public IConnector Connector { get; }
}

public sealed class ConnectorDeselectedEventArgs : EventArgs
{
    public ConnectorDeselectedEventArgs(IConnector connector)
    {
        Connector = connector;
    }

    public IConnector Connector { get; }
}

public sealed class ConnectorStartChangedEventArgs : EventArgs
{
    public ConnectorStartChangedEventArgs(IConnector connector)
    {
        Connector = connector;
    }

    public IConnector Connector { get; }
}

public sealed class ConnectorEndChangedEventArgs : EventArgs
{
    public ConnectorEndChangedEventArgs(IConnector connector)
    {
        Connector = connector;
    }

    public IConnector Connector { get; }
}

public interface IConnector
{
    string Name { get; set; }
    IDrawingNode Parent { get; set; }
    ConnectorOrientation Orientation { get; set; }
    IPin Start { get; set; }
    IPin End { get; set; }
    double Offset { get; set; }
    bool CanSelect();
    bool CanRemove();
    event EventHandler<ConnectorCreatedEventArgs> Created;
    event EventHandler<ConnectorRemovedEventArgs> Removed;
    event EventHandler<ConnectorSelectedEventArgs> Selected;
    event EventHandler<ConnectorDeselectedEventArgs> Deselected;
    event EventHandler<ConnectorStartChangedEventArgs> StartChanged;
    event EventHandler<ConnectorEndChangedEventArgs> EndChanged;
    void OnCreated();
    void OnRemoved();
    void OnSelected();
    void OnDeselected();
    void OnStartChanged();
    void OnEndChanged();
}
