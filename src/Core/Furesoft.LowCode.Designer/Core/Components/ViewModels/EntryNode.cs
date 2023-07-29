using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
public class EntryNode : VisualNode
{
    public EntryNode() : base("Entry")
    {
    }
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin FlowPin { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(FlowPin, cancellationToken: cancellationToken);
    }
}
