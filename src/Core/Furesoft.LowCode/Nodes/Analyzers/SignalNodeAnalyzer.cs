namespace Furesoft.LowCode.Nodes.Analyzers;

public class SignalNodeAnalyzer : GraphAnalyzerBase<SignalNode>
{
    public override void Analyze(SignalNode node)
    {
        if (!AnalyzerContext.HasConnection(node.OutputPin))
        {
            AddInfo("No node connected to Signal node", node);
        }
    }
}
