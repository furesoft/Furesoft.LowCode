using System.ComponentModel;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Designer.Core.Components.Analyzers;
using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Designer.Core.Components.ViewModels;

[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
[GraphAnalyzer(typeof(EntryNodeAnalyzer))]
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
