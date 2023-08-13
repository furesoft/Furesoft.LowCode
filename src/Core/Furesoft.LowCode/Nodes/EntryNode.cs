using System.ComponentModel;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes;

[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
[GraphAnalyzer(typeof(EntryNodeAnalyzer))]
public class EntryNode : OutputNode
{
    [Browsable(false)]
    public new bool ShowDescription { get; set; }
    
    public EntryNode() : base("Entry")
    {
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken);
    }
}
