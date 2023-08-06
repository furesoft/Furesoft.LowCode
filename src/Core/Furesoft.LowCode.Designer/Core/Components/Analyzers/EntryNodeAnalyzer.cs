using Furesoft.LowCode.Analyzing;

namespace Furesoft.LowCode.Designer.Core.Components.Analyzers;

public class EntryNodeAnalyzer : GraphAnalyzer<EntryNode>
{
    public override void Analyze(EntryNode node)
    {
        var hasSignalNodes = node.Drawing
            .GetNodes<SignalNode>()
            .Any();

        if (!node.GetOutputs(node.OutputPin).Any() && !hasSignalNodes)
        {
            AddInfo("No node connected to entry node", node);
        }
    }
}
