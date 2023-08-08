using System.ComponentModel;
using System.Runtime.CompilerServices;
using Furesoft.LowCode.Designer;

namespace Furesoft.LowCode.Analyzing;

public class AnalyzerContext
{
    private readonly CustomNodeViewModel _viewModel;

    public AnalyzerContext(CustomNodeViewModel viewModel)
    {
        _viewModel = viewModel;
    }

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
    /// Check if a node is connected before the current node
    /// </summary>
    /// <param name="maxIndirection">Maxmimum iterations to check. -1 disables the maxIndirection</param>
    /// <typeparam name="T">The node type to check for</typeparam>
    /// <returns></returns>
    public bool IsInputConnected<T>(int maxIndirection = -1)
    {
        return IsConnectedRecursive<T>(PinMode.Input, maxIndirection, _viewModel);
    }
    
    /// <summary>
    /// Check if a node is connected after the current node
    /// </summary>
    /// <param name="maxIndirection">Maxmimum iterations to check. -1 disables the maxIndirection</param>
    /// <typeparam name="T">The node type to check for</typeparam>
    /// <returns></returns>
    public bool IsOutputConnected<T>(int indirectionLength = -1)
    {
        return IsConnectedRecursive<T>(PinMode.Output, indirectionLength, _viewModel);
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
                var indireciton = indirectionLength == - 1 ? indirectionLength : indirectionLength - 1;
                
                if (connection.DefiningNode.GetType() == type)
                {
                    return true;
                }
                
                if (IsConnectedRecursive<T>(mode,  indireciton, connection))
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

    public bool IsNodePresentInGraph<T>()
        where T : EmptyNode
    {
        return _viewModel.DefiningNode.Drawing
            .GetNodes<T>()
            .Any();
    }

    public bool HasConnection(IInputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return AdjancencyMatrix[_viewModel].Any(_=> _.Mode == PinMode.Output);
    }

    public bool HasConnection(IOutputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return AdjancencyMatrix[_viewModel].Any(_=> _.Mode == PinMode.Output);
    }
}
