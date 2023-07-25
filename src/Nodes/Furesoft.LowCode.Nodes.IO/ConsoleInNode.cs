using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Core;
using Furesoft.LowCode.Core.NodeBuilding;
using NodeEditor.Model;

namespace Furesoft.LowCode.Nodes.IO;

[NodeCategory("IO")]
[Description("Read a value from the console")]
public class ConsoleInNode : VisualNode
{
    [Description("The input from the console")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Output { get; set; }

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    
    public ConsoleInNode() : base("Console In")
    {
    }

    public override Task Execute()
    {
        var input = Console.ReadLine();
        SetOutVariable(Output, input);

        return ContinueWith(OutputPin);
    }
}
