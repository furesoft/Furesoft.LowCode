using Furesoft.LowCode.Analyzing;

namespace Furesoft.LowCode.Nodes.Analyzers;

public class GenericNodeAnalyzer : GraphAnalyzerBase<EmptyNode>
{
    public override void Analyze(EmptyNode node)
    {
        var isEntryConnected = AnalyzerContext.IsInputConnected<EntryNode>();
        var isSignalConnected = AnalyzerContext.IsInputConnected<SignalNode>();

        if (!(isEntryConnected || isSignalConnected))
        {
            AddError("This execution path will never be executet. Connect it with Entry or Signal", node);
        }
    }
}
