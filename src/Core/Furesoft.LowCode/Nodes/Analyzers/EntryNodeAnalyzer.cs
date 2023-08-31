namespace Furesoft.LowCode.Nodes.Analyzers;

public class EntryNodeAnalyzer : GraphAnalyzerBase<EntryNode>
{
    public override void Analyze(EntryNode node)
    {
        var hasSignalNodes = AnalyzerContext.IsNodePresentInGraph<SignalNode>();

        if (!AnalyzerContext.HasConnection(node.OutputPin) && !hasSignalNodes)
        {
            AddInfo("No node connected to entry node", node);
        }
    }
}
