using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Furesoft.LowCode.Editor.Behaviors;

public class InsertTemplateOnDoubleTappedBehavior : Behavior<Control>
{
    public static readonly StyledProperty<IDrawingNode> DrawingProperty =
        AvaloniaProperty.Register<InsertTemplateOnDoubleTappedBehavior, IDrawingNode>(nameof(Drawing));

    public IDrawingNode Drawing
    {
        get => GetValue(DrawingProperty);
        set => SetValue(DrawingProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is not null)
        {
            AssociatedObject.DoubleTapped += DoubleTapped;
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject is not null)
        {
            AssociatedObject.DoubleTapped -= DoubleTapped;
        }
    }

    private void DoubleTapped(object sender, RoutedEventArgs args)
    {
        if (AssociatedObject is {DataContext: INodeTemplate template} && Drawing is { } drawing)
        {
            var node = drawing.Clone(template.Template);
            if (node is not null)
            {
                node.Parent = drawing;
                node.Move(drawing.Width / 2, drawing.Height / 2);
                drawing.Nodes?.Add(node);
                node.OnCreated();
            }
        }
    }
}
