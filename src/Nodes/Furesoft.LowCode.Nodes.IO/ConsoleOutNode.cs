using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Nodes.IO;

[DataContract(IsReference = true)]
[Description("Write a text to the console")]
[NodeCategory("IO/Console")]
[NodeView(typeof(IconNodeView), "M0 0 42 0 42 36 0 36 0 0ZM3 6 3 33 39 33 39 6 3 6ZM6.75 11 11.5 11 16.25 17.5 11.5 24 6.75 24 11.5 17.5 6.75 11Z")]
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

    public override Task Execute(CancellationToken cancellationToken)
    {
        var msg= Evaluate<string>(Message);
       
        Console.WriteLine(msg);

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
