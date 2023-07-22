using System.Collections.ObjectModel;
using NodeEditor.Model;
using NodeEditor.Mvvm;
using NodeEditorDemo.Core.Components.ViewModels;

namespace NodeEditorDemo.Core;

internal static class BaseNodeFactory
{
    public static IDrawingNode CreateDrawing(string name = null)
    {
        var drawing = new DrawingNodeViewModel
        {
            X = 0,
            Y = 0,
            Name = name,
            Width = 90000,
            Height = 60000,
            Nodes = new ObservableCollection<INode>(),
            Connectors = new ObservableCollection<IConnector>(),
            EnableMultiplePinConnections = true,
            EnableSnap = true,
            SnapX = 10.0,
            SnapY = 10.0,
        };

        var entry = CreateEntry((drawing.Width / 2, drawing.Height / 2 - 275));
        entry.Parent = drawing;

        drawing.Nodes.Add(entry);

        return drawing;
    }
    
    public static CustomNodeViewModel CreateViewModel(VisualNode vm, (double x, double y) position,
        (double width, double height) size)
    {
        var node = new CustomNodeViewModel
        {
            X = position.x,
            Y = position.y,
            Width = size.width,
            Height = size.height,
            Pins = new ObservableCollection<IPin>(),
            Content = vm
        };

        return node;
    }
    
    private static INode CreateEntry((double x, double y) position)
    {
        var node = NodeFactory.CreateNode(new EntryNode(), position);
        node.IsRemovable = false;

        return node;
    }
}
