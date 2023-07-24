using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using NodeEditor.Model;

namespace Furesoft.LowCode.Core;

public class DynamicNode : VisualNode
{
    public readonly Dictionary<string, PinAlignment> Pins = new();
    
    [Browsable(false)]
    public Control View { get; set; }

    public DynamicNode(string label, Control view = null) : base(label)
    {
        View = view;

        TypeDescriptor.AddAttributes(this, new DescriptionAttribute("A simple dynamic node"));
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
