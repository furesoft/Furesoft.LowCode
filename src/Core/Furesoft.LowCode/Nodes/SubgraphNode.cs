using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes;

[IgnoreTemplate]
[NodeView(typeof(SubgraphView))]
public class SubgraphNode : DynamicNode
{
    public SubgraphNode(GraphItem graphItem) : base(null)
    {
        GraphItem = graphItem;
        Description = graphItem.Name;
        Label = graphItem.Name;

        AddPin("Input Flow", PinAlignment.Top, PinMode.Input);
        AddPin("Output Flow", PinAlignment.Bottom, PinMode.Output);
    }

    public GraphItem GraphItem { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var subevaluator = new Evaluator(GraphItem.Content);

        await subevaluator.Execute(cancellationToken);

        await ContinueWith("Output Flow", cancellationToken);
    }
}
