using System.ComponentModel;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer;

public sealed class GraphAnalyzer
{
    public IList<Message> Analyze(IDrawingNode drawing)
    {
        var messages = new List<Message>();
        
        foreach (var node in drawing.Nodes.OfType<CustomNodeViewModel>())
        {
            var analyzers = GetAnalyzers(node.DefiningNode);

            foreach (var analyzer in analyzers)
            {
                node.DefiningNode.Drawing = drawing;
                
                analyzer.Analyze(node.DefiningNode);
                messages.AddRange(analyzer.Messages);
            }
        }

        return messages;
    }

    public static IEnumerable<IGraphAnalyzer> GetAnalyzers(EmptyNode node)
    {
        return TypeDescriptor
            .GetAttributes(node)
            .OfType<GraphAnalyzerAttribute>()
            .Select(attr => attr.Type)
            .Select(_ => Activator.CreateInstance(_) as IGraphAnalyzer);
    }
}
