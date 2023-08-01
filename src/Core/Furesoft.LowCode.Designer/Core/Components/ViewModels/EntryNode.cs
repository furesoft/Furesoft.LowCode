using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[DataContract(IsReference = true)]
[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
public class EntryNode : OutputNode
{
    public EntryNode() : base("Entry")
    {
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
