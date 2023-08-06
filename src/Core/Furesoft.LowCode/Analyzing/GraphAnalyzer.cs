using Furesoft.LowCode.Designer;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Analyzing;

public sealed class GraphAnalyzer
{
    public IList<Message> Analyze(IDrawingNode drawing)
    {
        var messages = new List<Message>();
        
        foreach (var node in drawing.Nodes.OfType<CustomNodeViewModel>())
        {
            var analyzers = node.AnalyzerContext.GetAnalyzers(node.DefiningNode);

            foreach (var analyzer in analyzers)
            {
                node.DefiningNode.Drawing = drawing;

                analyzer.AnalyzerContext = node.AnalyzerContext;
                analyzer.Analyze(node.DefiningNode);
                messages.AddRange(analyzer.Messages);
            }
        }

        return messages;
    }
}
