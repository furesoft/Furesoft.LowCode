using System.ComponentModel;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes;

[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
[GraphAnalyzer(typeof(EntryNodeAnalyzerBase))]
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
