using Furesoft.LowCode.Analyzing;

namespace Furesoft.LowCode.Nodes.Analyzers;

public class GenericNodeAnalyzer : GraphAnalyzerBase<EmptyNode>
{
    public override void Analyze(EmptyNode node)
    {
        var isEntryConnected = AnalyzerContext.IsInputConnected<EntryNode>();
        var isSignalConnected = AnalyzerContext.IsInputConnected<SignalNode>();
        var hasCycle = AnalyzerContext.HasCycle(node);
        
        if (!(isEntryConnected || isSignalConnected))
        {
            AddError("This execution path will never be executed. Connect it with Entry or Signal", node);
        }
    }
}
