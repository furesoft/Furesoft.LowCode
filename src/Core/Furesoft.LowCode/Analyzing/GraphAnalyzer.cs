using System.Diagnostics;
using Furesoft.LowCode.Designer;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Analyzing;

public sealed class GraphAnalyzer
{
    private readonly Dictionary<CustomNodeViewModel, List<CustomNodeViewModel>> adjacencyMatrix = new();
    
    public IList<Message> Analyze(IDrawingNode drawing)
    {
        BuildAdjanceMatrix(drawing);
        
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
    
    private void BuildAdjanceMatrix(IDrawingNode graphNode)
    {
        adjacencyMatrix.Clear();
        
        foreach (var node in graphNode.Nodes.OfType<CustomNodeViewModel>())
        {
            adjacencyMatrix[node] = new();

            var connectedNodes = GetConnenctedNodes(node, graphNode);

            adjacencyMatrix[node].AddRange(connectedNodes);
        }
        
        PrintAdjanceMatrix();
    }
    
    private IEnumerable<CustomNodeViewModel> GetConnenctedNodes(CustomNodeViewModel node, IDrawingNode graphNode)
    {
        var connections = from connection in graphNode.Connectors
            where ((CustomNodeViewModel)connection.Start.Parent).DefiningNode == node.DefiningNode
                  || ((CustomNodeViewModel)connection.End.Parent).DefiningNode == node.DefiningNode
            select connection;

        var nodes = new List<CustomNodeViewModel>();
        
        foreach (var connection in connections)
        {
            var start = connection.Start.Parent as CustomNodeViewModel;
            var end = connection.End.Parent as CustomNodeViewModel;

            if (start.DefiningNode == node.DefiningNode)
            {
                nodes.Add(end);
            }
            else if (end.DefiningNode == node.DefiningNode)
            {
                nodes.Add(start);
            }
        }

        return nodes;
    }

    private void PrintAdjanceMatrix()
    {
        Debug.WriteLine("Adjacency Matrix:");
        foreach (var sourceNode in adjacencyMatrix.Keys)
        {
            var targetNodes = adjacencyMatrix[sourceNode];

            Debug.WriteLine($"{sourceNode.Name} -> {string.Join(',', targetNodes.Select(_=> _.DefiningNode.GetClassName()))}");
        }
    }
}
