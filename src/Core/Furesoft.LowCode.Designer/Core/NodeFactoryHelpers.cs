using System.Collections.ObjectModel;
using Furesoft.LowCode.Designer.Core.Components.ViewModels;
using Furesoft.LowCode.Editor;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Designer.Core;

public partial class NodeFactory
{
    public IDrawingNode CreateDrawing(string name = null)
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
            EnableSnap = true,
            SnapX = 30.0,
            SnapY = 30.0,
            GridCellHeight = 30,
            GridCellWidth = 30,
            EnableGrid = true
        };

        var entryX = SnapHelper.Snap(drawing.Width / 2 - 175, drawing.SnapX);
        var entryY = SnapHelper.Snap(drawing.Height / 2 - 200, drawing.SnapY);

        CreateEntry((entryX, entryY), drawing);

        return drawing;
    }

    private static CustomNodeViewModel CreateViewModel(EmptyNode vm, (double x, double y) position,
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

    private static void CreateEntry((double x, double y) position, IDrawingNode drawing)
    {
        var node = CreateNode(new EntryNode(), position);
        node.IsRemovable = false;
        node.Parent = drawing;

        drawing.Nodes!.Add(node);
    }

    private static double CalculateSinglePin(double width, int pinCount, int i)
    {
        return width / (pinCount + 1) * (i + 1);
    }

    private static void AddPins(
        IEnumerable<(string Name, PinAlignment Alignment, PinMode Mode, bool MultipleConnections)> pins,
        NodeViewModel viewModel, Func<int, (double, double)> positionMapper)
    {
        var pinArray = pins as (string Name, PinAlignment Alignment, PinMode Mode, bool MultipleConnections)[] ?? pins.ToArray();
        
        for (var i = 0; i < pinArray.Length; i++)
        {
            var pin = pinArray[i];
            var (baseX, baseY) = positionMapper(i);

            viewModel.AddPin((baseX, baseY), (PinSize, PinSize), pin.Mode, (Editor.Model.PinAlignment)pin.Alignment,
                pin.Name, pin.MultipleConnections);
        }
    }
}
