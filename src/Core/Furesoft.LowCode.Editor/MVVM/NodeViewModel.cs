using CommunityToolkit.Mvvm.ComponentModel;

namespace Furesoft.LowCode.Editor.MVVM;

[ObservableObject]
public partial class NodeViewModel : INode
{
    [ObservableProperty] private object? _content;
    [ObservableProperty] private string? _description;
    [ObservableProperty] private double _height;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private INode? _parent;
    [ObservableProperty] private IList<IPin>? _pins;
    [ObservableProperty] private double _width;
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;

    public event EventHandler<NodeCreatedEventArgs>? Created;

    public event EventHandler<NodeRemovedEventArgs>? Removed;

    public event EventHandler<NodeMovedEventArgs>? Moved;

    public event EventHandler<NodeSelectedEventArgs>? Selected;

    public event EventHandler<NodeDeselectedEventArgs>? Deselected;

    public event EventHandler<NodeResizedEventArgs>? Resized;

    public virtual bool CanSelect()
    {
        return true;
    }

    public virtual bool CanRemove()
    {
        return true;
    }

    public virtual bool CanMove()
    {
        return true;
    }

    public virtual bool CanResize()
    {
        return true;
    }

    public virtual void Move(double deltaX, double deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }

    public virtual void Resize(double deltaX, double deltaY, NodeResizeDirection direction)
    {
        // TODO: Resize
    }

    public virtual void OnCreated()
    {
        Created?.Invoke(this, new(this));
    }

    public virtual void OnRemoved()
    {
        Removed?.Invoke(this, new(this));
    }

    public virtual void OnMoved()
    {
        Moved?.Invoke(this, new(this, _x, _y));
    }

    public virtual void OnSelected()
    {
        Selected?.Invoke(this, new(this));
    }

    public virtual void OnDeselected()
    {
        Deselected?.Invoke(this, new(this));
    }

    public virtual void OnResized()
    {
        Resized?.Invoke(this, new(this, _x, _y, _width, _height));
    }
}
