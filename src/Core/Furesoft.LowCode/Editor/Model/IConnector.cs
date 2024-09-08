namespace Furesoft.LowCode.Editor.Model;

public sealed class ConnectorCreatedEventArgs(IConnector connector) : EventArgs
{
    public IConnector Connector { get; } = connector;
}

public sealed class ConnectorRemovedEventArgs(IConnector connector) : EventArgs
{
    public IConnector Connector { get; } = connector;
}

public sealed class ConnectorSelectedEventArgs(IConnector connector) : EventArgs
{
    public IConnector Connector { get; } = connector;
}

public sealed class ConnectorDeselectedEventArgs(IConnector connector) : EventArgs
{
    public IConnector Connector { get; } = connector;
}

public sealed class ConnectorStartChangedEventArgs(IConnector connector) : EventArgs
{
    public IConnector Connector { get; } = connector;
}

public sealed class ConnectorEndChangedEventArgs(IConnector connector) : EventArgs
{
    public IConnector Connector { get; } = connector;
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
