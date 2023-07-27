using System.Collections.ObjectModel;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Editor.MVVM;

public static class NodeViewModelExtensions
{
    public static IPin AddPin(this NodeViewModel node, double x, double y, double width, double height,PinMode mode, PinAlignment alignment = PinAlignment.None, string? name = null)
    {
        var pin = new PinViewModel
        {
            Name = name,
            Parent = node,
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Alignment = alignment,
            Mode = mode
        };

        node.Pins ??= new ObservableCollection<IPin>();
        node.Pins.Add(pin);

        return pin;
    }
}
