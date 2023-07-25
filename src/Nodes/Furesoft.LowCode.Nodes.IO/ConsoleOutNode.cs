using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Core;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;

namespace Furesoft.LowCode.Nodes.IO;

[NodeCategory("IO")]
[Description("Write a text to the console")]
public class ConsoleOutNode : VisualNode
{
    [Description("The text to display")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Message { get; set; }

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    
    public ConsoleOutNode() : base("Console Out")
    {
    }

    public override Task Execute()
    {
        var msg= Evaluate<string>(Message);
       
        Console.WriteLine(msg);

        return ContinueWith(OutputPin);
    }
}
