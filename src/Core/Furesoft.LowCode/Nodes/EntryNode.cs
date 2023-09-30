using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes;

[IgnoreTemplate]
[NodeView(typeof(EntryView))]
[Description("The starting node of the graph")]
[GraphAnalyzer(typeof(EntryNodeAnalyzer))]
public class EntryNode : OutputNode
{
    public EntryNode() : base("Entry")
    {
    }

    [Browsable(false)] public new bool ShowDescription { get; set; }

    public override void Compile(CodeWriter builder)
    {
        builder.BeginFunctionDecl(Drawing.Name);

        builder.BeginBlock();

        CompilePin(OutputPin, builder);

        builder.EndBlock();
    }
}
