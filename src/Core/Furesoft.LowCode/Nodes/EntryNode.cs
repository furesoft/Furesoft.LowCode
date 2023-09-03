using System.ComponentModel;
using System.Text;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes;

[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
[GraphAnalyzer(typeof(EntryNodeAnalyzer))]
public class EntryNode : OutputNode, ICompilationNode
{
    public EntryNode() : base("Entry")
    {
    }

    [Browsable(false)] public new bool ShowDescription { get; set; }

    public void Compile(StringBuilder builder)
    {
        CompilePin(OutputPin, builder);
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken);
    }
}
