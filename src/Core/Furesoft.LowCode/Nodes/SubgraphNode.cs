using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes;

[IgnoreTemplate]
[NodeIcon(
    "M2.5 12.8125Q1.5938 12.8125.9532 12.1719T.3125 10.625Q.3125 9.7188.9532 9.0782T2.5 8.4375Q2.7813 8.4375 3.0469 8.5078T3.5469 8.7032L5.9375 6.3125V4.5938Q5.25 4.3906 4.8125 3.8203T4.375 2.5Q4.375 1.5938 5.0156.9532T6.5625.3125Q7.4688.3125 8.1094.9532T8.75 2.5Q8.75 3.25 8.3125 3.8203T7.1875 4.5938V6.3125L9.5938 8.7032Q9.8282 8.5782 10.086 8.5078T10.625 8.4375Q11.5313 8.4375 12.1719 9.0782T12.8125 10.625Q12.8125 11.5313 12.1719 12.1719T10.625 12.8125Q9.7188 12.8125 9.0782 12.1719T8.4375 10.625Q8.4375 10.3438 8.5078 10.0782T8.7032 9.5782L6.5625 7.4375 4.4219 9.5782Q4.5469 9.8125 4.6172 10.0782T4.6875 10.625Q4.6875 11.5313 4.0469 12.1719T2.5 12.8125ZM10.625 11.5625Q11.0157 11.5625 11.289 11.289T11.5625 10.625Q11.5625 10.2344 11.289 9.961T10.625 9.6875Q10.2344 9.6875 9.961 9.961T9.6875 10.625Q9.6875 11.0157 9.961 11.289T10.625 11.5625ZM6.5625 3.4375Q6.9531 3.4375 7.2265 3.1641T7.5 2.5Q7.5 2.1094 7.2265 1.8359T6.5625 1.5625Q6.1719 1.5625 5.8985 1.8359T5.625 2.5Q5.625 2.8906 5.8985 3.1641T6.5625 3.4375ZM2.5 11.5625Q2.8906 11.5625 3.1641 11.289T3.4375 10.625Q3.4375 10.2344 3.1641 9.961T2.5 9.6875Q2.1094 9.6875 1.8359 9.961T1.5625 10.625Q1.5625 11.0157 1.8359 11.289T2.5 11.5625Z")]
public class SubgraphNode : DynamicNode
{
    private readonly GraphItem _graphItem;

    public SubgraphNode(GraphItem graphItem) : base(null)
    {
        _graphItem = graphItem;
        Description = graphItem.Name;
        Label = graphItem.Name;

        AddPin("Input Flow", PinAlignment.Top, PinMode.Input);
        AddPin("Output Flow", PinAlignment.Bottom, PinMode.Output);
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var subevaluator = new Evaluator(_graphItem.Content);

        await subevaluator.Execute(cancellationToken);

        await ContinueWith("Output Flow", cancellationToken);
    }
}
