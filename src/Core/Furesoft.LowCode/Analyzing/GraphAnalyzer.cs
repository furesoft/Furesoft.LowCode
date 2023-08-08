using System.Diagnostics;
using Furesoft.LowCode.Designer;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Analyzing;

public sealed class GraphAnalyzer
{
    private readonly AdjancencyMatrix _adjacencyMatrix = new();
    
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
                analyzer.AnalyzerContext.AdjancencyMatrix = _adjacencyMatrix;
                analyzer.Analyze(node.DefiningNode);
                messages.AddRange(analyzer.Messages);
            }
        }

        return messages;
    }
    
    private void BuildAdjanceMatrix(IDrawingNode graphNode)
    {
        _adjacencyMatrix.Clear();
        
        foreach (var node in graphNode.Nodes.OfType<CustomNodeViewModel>())
        {
            _adjacencyMatrix[node] = new();

            var connectedNodes = GetConnenctedNodes(node, graphNode);

            _adjacencyMatrix[node].AddRange(connectedNodes);
        }
        
        PrintAdjanceMatrix();
    }
    
    private IEnumerable<(CustomNodeViewModel, PinMode)> GetConnenctedNodes(CustomNodeViewModel node, IDrawingNode graphNode)
    {
        var connections = from connection in graphNode.Connectors
            where ((CustomNodeViewModel)connection.Start.Parent).DefiningNode == node.DefiningNode
                  || ((CustomNodeViewModel)connection.End.Parent).DefiningNode == node.DefiningNode
            select connection;

        var nodes = new List<(CustomNodeViewModel, PinMode)>();
        
        foreach (var connection in connections)
        {
            var start = connection.Start.Parent as CustomNodeViewModel;
            var end = connection.End.Parent as CustomNodeViewModel;

            if (start!.DefiningNode == node.DefiningNode)
            {
                nodes.Add((end, connection.Start.Mode));
            }
            else if (end!.DefiningNode == node.DefiningNode)
            {
                nodes.Add((start, connection.End.Mode));
            }
        }

        return nodes;
    }

    private void PrintAdjanceMatrix()
    {
        Debug.WriteLine("Adjacency Matrix:");
        
        foreach (var sourceNode in _adjacencyMatrix.Keys)
        {
            var targetNodes = _adjacencyMatrix[sourceNode];

            Debug.WriteLine($"{sourceNode.Name} -> {string.Join(',', targetNodes.Select(_=> _.Node.DefiningNode.GetClassName() + " " + _.Mode))}");
        }
    }
}
