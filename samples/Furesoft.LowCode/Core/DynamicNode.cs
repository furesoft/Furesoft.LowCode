using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core;

public class DynamicNode : VisualNode
{
    public readonly Dictionary<string, PinAlignment> Pins = new();
    public Control View { get; set; }

    public DynamicNode(string label, Control view = null) : base(label)
    {
        View = view;
    }

    public void AddPin(string name, PinAlignment alignment)
    {
        Pins.Add(name, alignment);
    }

    public override Task Execute()
    {
        throw new System.NotImplementedException();
    }
}
