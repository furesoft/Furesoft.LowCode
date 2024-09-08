namespace Furesoft.LowCode.Editor.Model;

public sealed class NodeCreatedEventArgs(INode node) : EventArgs
{
    public INode Node { get; } = node;
}

public sealed class NodeRemovedEventArgs(INode node) : EventArgs
{
    public INode Node { get; } = node;
}

public sealed class NodeMovedEventArgs(INode node, double x, double y) : EventArgs
{
    public INode Node { get; } = node;

    public double X { get; } = x;

    public double Y { get; } = y;
}

public sealed class NodeSelectedEventArgs(INode node) : EventArgs
{
    public INode Node { get; } = node;
}

public sealed class NodeDeselectedEventArgs(INode node) : EventArgs
{
    public INode Node { get; } = node;
}

public sealed class NodeResizedEventArgs(INode node, double x, double y, double width, double height)
    : EventArgs
{
    public INode Node { get; } = node;

    public double X { get; } = x;

    public double Y { get; } = y;

    public double Width { get; } = width;

    public double Height { get; } = height;
}

public interface INode
{
    string Name { get; set; }
    INode Parent { get; set; }
    double X { get; set; }
    double Y { get; set; }
    double Width { get; set; }
    double Height { get; set; }
    object Content { get; set; }
    IList<IPin> Pins { get; set; }
    bool CanSelect();
    bool CanRemove();
    bool CanMove();
    bool CanResize();
    void Move(double deltaX, double deltaY);
    void Resize(double deltaX, double deltaY, NodeResizeDirection direction);
    event EventHandler<NodeCreatedEventArgs> Created;
    event EventHandler<NodeRemovedEventArgs> Removed;
    event EventHandler<NodeMovedEventArgs> Moved;
    event EventHandler<NodeSelectedEventArgs> Selected;
    event EventHandler<NodeDeselectedEventArgs> Deselected;
    event EventHandler<NodeResizedEventArgs> Resized;
    void OnCreated();
    void OnRemoved();
    void OnMoved();
    void OnSelected();
    void OnDeselected();
    void OnResized();
}
