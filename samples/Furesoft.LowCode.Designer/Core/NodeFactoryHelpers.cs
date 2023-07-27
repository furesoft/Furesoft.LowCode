using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Furesoft.LowCode.Designer.Core.Components.ViewModels;
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

    private static CustomNodeViewModel CreateViewModel(VisualNode vm, (double x, double y) position,
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
        var node = CreateNode(new EntryNode(), position);
        node.IsRemovable = false;

        return node;
    }

    private static double CalculateSinglePin(double width, int pinCount, int i)
    {
        return width / (pinCount + 1) * (i + 1);
    }

    private static void AddPins(double pinSize, IEnumerable<KeyValuePair<string, PinAlignment>> pins,
        CustomNodeViewModel viewModel, Func<int, (double, double)> positionMapper)
    {
        for (int i = 0; i < pins.Count(); i++)
        {
            var pin = pins.Skip(i).First();

            (double baseX, double baseY) = positionMapper(i);

            viewModel.AddPin(baseX, baseY, pinSize, pinSize, pin.Value, pin.Key);
        }
    }
}
