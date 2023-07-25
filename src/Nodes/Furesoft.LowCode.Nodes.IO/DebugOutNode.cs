using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using Furesoft.LowCode.Core;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;

namespace Furesoft.LowCode.Nodes.IO;

[NodeCategory("IO/Debug")]
[Description("Write a text to the debug log")]
public class DebugOutNode : VisualNode
{
    private string _message;

    [Description("The text to display")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    
    public DebugOutNode() : base("Debug Out")
    {
    }

    public override Task Execute()
    {
        var msg= Evaluate<string>(Message);
       
        Debug.WriteLine(msg);

        return ContinueWith(OutputPin);
    }
}
