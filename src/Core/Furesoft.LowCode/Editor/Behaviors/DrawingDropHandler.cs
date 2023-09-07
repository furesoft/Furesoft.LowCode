using Avalonia.Input;
using Furesoft.LowCode.ProjectSystem.Items;

namespace Furesoft.LowCode.Editor.Behaviors;

public class DrawingDropHandler : DefaultDropHandler
{
    public static readonly StyledProperty<Control> RelativeToProperty =
        AvaloniaProperty.Register<DrawingDropHandler, Control>(nameof(RelativeTo));

    public Control RelativeTo
    {
        get => GetValue(RelativeToProperty);
        set => SetValue(RelativeToProperty, value);
    }

    private bool Validate(IDrawingNode drawing, object sender, DragEventArgs e, bool bExecute)
    {
        var relativeTo = RelativeTo ?? sender as Control;
        if (relativeTo is null)
        {
            return false;
        }

        var point = GetPosition(relativeTo, e);

        if (relativeTo is DrawingNode drawingNode)
        {
            point = SnapHelper.Snap(point, drawingNode.SnapX, drawingNode.SnapY, drawingNode.EnableSnap);
        }

        if (e.Data.Contains(DataFormats.Text))
        {
            var text = e.Data.GetText();

            if (bExecute && text is not null)
            {
                // TODO: text
            }

            return true;
        }

        foreach (var format in e.Data.GetDataFormats())
        {
            var data = e.Data.Get(format);

            switch (data)
            {
                case INodeTemplate template:
                    if (!bExecute)
                    {
                        return true;
                    }

                    var node = drawing.Clone(template.Template);
                    PlaceNode(drawing, node, point);

                    return true;
                case GraphItem item:
                    if (!bExecute)
                    {
                        return true;
                    }

                    var factory = new NodeFactory();
                    var subgraphNode = factory.CreateSubGraphNode(item);
                    PlaceNode(drawing, subgraphNode, point);

                    return true;
            }
        }

        if (e.Data.Contains(DataFormats.Files))
        {
            // ReSharper disable once UnusedVariable
            var files = e.Data.GetFiles()?.ToArray();
            if (bExecute)
            {
                // TODO: files, point.X, point.Y
            }

            return true;
        }

        return false;
    }

    private static void PlaceNode(IDrawingNode drawing, INode node, Point point)
    {
        if (node is not null)
        {
            node.Parent = drawing;
            node.Move(point.X, point.Y);
            drawing.Nodes?.Add(node);
            node.OnCreated();
        }
    }

    public override bool Validate(object sender, DragEventArgs e, object sourceContext, object targetContext,
        object state)
    {
        if (targetContext is IDrawingNode drawing)
        {
            return Validate(drawing, sender, e, false);
        }

        return false;
    }

    public override bool Execute(object sender, DragEventArgs e, object sourceContext, object targetContext,
        object state)
    {
        if (targetContext is IDrawingNode drawing)
        {
            return Validate(drawing, sender, e, true);
        }

        return false;
    }
}
