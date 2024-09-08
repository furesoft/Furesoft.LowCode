using System.ComponentModel;
using System.Runtime.CompilerServices;
using Furesoft.LowCode.Designer;

namespace Furesoft.LowCode.Analyzing;

public class AnalyzerContext(CustomNodeViewModel viewModel)
{
    public AdjancencyMatrix AdjancencyMatrix { get; set; }

    public IEnumerable<IGraphAnalyzer> GetAnalyzers(EmptyNode node)
    {
        return TypeDescriptor
            .GetAttributes(node)
            .OfType<GraphAnalyzerAttribute>()
            .Select(attr => attr.Type)
            .Select(_ => Activator.CreateInstance(_) as IGraphAnalyzer);
    }

    /// <summary>
    ///     Check if a node is connected before the current node
    /// </summary>
    /// <param name="maxIndirection">Maxmimum iterations to check. -1 disables the maxIndirection</param>
    /// <typeparam name="T">The node type to check for</typeparam>
    /// <returns></returns>
    public bool IsInputConnected<T>(int maxIndirection = -1)
    {
        return IsConnectedRecursive<T>(PinMode.Input, maxIndirection, viewModel);
    }

    /// <summary>
    ///     Check if a node is connected after the current node
    /// </summary>
    /// <param name="maxIndirection">Maxmimum iterations to check. -1 disables the maxIndirection</param>
    /// <typeparam name="T">The node type to check for</typeparam>
    /// <returns></returns>
    public bool IsOutputConnected<T>(int indirectionLength = -1)
    {
        return IsConnectedRecursive<T>(PinMode.Output, indirectionLength, viewModel);
    }

    private bool IsConnectedRecursive<T>(PinMode mode, int indirectionLength, CustomNodeViewModel node)
    {
        if (indirectionLength == 0)
        {
            return false;
        }

        var type = typeof(T);
        var currentNodeConnections = GetNodeConnections(mode, node);

        // Ignore nodes without pins
        if (!currentNodeConnections.Any())
        {
            return true;
        }

        foreach (var currentNodeInput in currentNodeConnections)
        {
            if (currentNodeInput.DefiningNode.GetType() == type)
            {
                return true;
            }

            var connections = GetNodeConnections(PinMode.Input, currentNodeInput);

            foreach (var connection in connections)
            {
                // If indirectionLength == -1 ignore it
                var indireciton = indirectionLength == -1 ? indirectionLength : indirectionLength - 1;

                if (connection.DefiningNode.GetType() == type)
                {
                    return true;
                }

                if (IsConnectedRecursive<T>(mode, indireciton, connection))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerable<CustomNodeViewModel> GetNodeConnections(PinMode mode, CustomNodeViewModel node)
    {
        var currentConnections = AdjancencyMatrix[node];
        var result = new List<CustomNodeViewModel>();

        foreach (var currentConnection in currentConnections)
        {
            if (currentConnection.Mode != mode)
            {
                continue;
            }

            result.Add(currentConnection.Node);
        }

        return result;
    }

    private IEnumerable<EmptyNode> GetNodeConnections(PinMode mode, EmptyNode node)
    {
        var currentConnections = GetNodeConnectionsFromMatrix(node);
        var result = new List<EmptyNode>();

        foreach (var currentConnection in currentConnections)
        {
            if (currentConnection.Mode != mode)
            {
                continue;
            }

            result.Add(currentConnection.Node.DefiningNode);
        }

        return result;
    }

    private List<(CustomNodeViewModel Node, PinMode Mode)> GetNodeConnectionsFromMatrix(EmptyNode node)
    {
        foreach (var key in AdjancencyMatrix)
        {
            if (key.Key.DefiningNode == node)
            {
                return key.Value;
            }
        }

        return new();
    }

    public bool IsNodePresentInGraph<T>()
        where T : EmptyNode
    {
        return viewModel.DefiningNode.Drawing
            .GetNodes<T>()
            .Any();
    }

    public bool HasConnection(IInputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return AdjancencyMatrix[viewModel].Any(_ => _.Mode == PinMode.Output);
    }

    public bool HasConnection(IOutputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return AdjancencyMatrix[viewModel].Any(_ => _.Mode == PinMode.Output);
    }

    public IEnumerable<EmptyNode> FindDisconnectedNodes()
    {
        return viewModel.DefiningNode.Drawing.Nodes
            .OfType<EmptyNode>()
            .Where(node => !IsInputConnected<EmptyNode>() && !IsOutputConnected<EmptyNode>())
            .ToList();
    }

    public IEnumerable<CustomNodeViewModel> FindCommonNeighbors(CustomNodeViewModel node1, CustomNodeViewModel node2)
    {
        var neighbors1 = GetNodeConnections(PinMode.Input, node1)
            .Union(GetNodeConnections(PinMode.Output, node1))
            .Distinct();

        var neighbors2 = GetNodeConnections(PinMode.Input, node2)
            .Union(GetNodeConnections(PinMode.Output, node2))
            .Distinct();

        return neighbors1.Intersect(neighbors2);
    }

    public bool HasCycle(EmptyNode startNode)
    {
        var visited = new HashSet<EmptyNode>();
        var recursionStack = new HashSet<EmptyNode>();

        return HasCycle(startNode, visited, recursionStack);
    }

    private bool HasCycle(EmptyNode node, HashSet<EmptyNode> visited, HashSet<EmptyNode> recursionStack)
    {
        if (recursionStack.Contains(node))
        {
            return true; // Found a back edge, indicating a cycle
        }

        if (visited.Contains(node))
        {
            return false; // Already visited, no cycle
        }

        visited.Add(node);
        recursionStack.Add(node);

        var neighbors = GetNodeConnections(PinMode.Output, node);

        if (neighbors.Any(neighbor => HasCycle(neighbor, visited, recursionStack)))
        {
            return true; // Found a cycle in the neighbor's subtree
        }

        recursionStack.Remove(node);

        return false; // No cycle found in the subtree rooted at this node
    }
}
