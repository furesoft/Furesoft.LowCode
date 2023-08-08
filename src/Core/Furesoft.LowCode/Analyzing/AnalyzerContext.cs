using System.ComponentModel;
using System.Runtime.CompilerServices;
using Furesoft.LowCode.Designer;
using Furesoft.LowCode.Nodes;

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

    public bool IsInputConnected<T>(uint indirectionLength = 1)
    {
        return IsConnectedRecursive<T>(PinMode.Input, indirectionLength, _viewModel);
    }
    
    public bool IsOutputConnected<T>(uint indirectionLength = 1)
    {
        return IsConnectedRecursive<T>(PinMode.Input, indirectionLength, _viewModel);
    }

    private bool IsConnectedRecursive<T>(PinMode mode, uint indirectionLength, CustomNodeViewModel node)
    {
        if (indirectionLength == 0)
        {
            return false;
        }

        var type = typeof(T);
        var currentNodeInputs = GetNodeConnections(mode, node);

        foreach (var currentNodeInput in currentNodeInputs)
        {
            if (currentNodeInput.DefiningNode.GetType() == type)
            {
                return true;
            }

            var connections = GetNodeConnections(PinMode.Input, currentNodeInput);

            foreach (var connection in connections)
            {
                if (connection.DefiningNode is not EntryNode && IsConnectedRecursive<T>(mode, indirectionLength - 1, connection))
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
        return _viewModel.DefiningNode.GetConnectedNodes(pinMembername, PinMode.Input).Any();
    }

    public bool HasConnection(IOutputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return _viewModel.DefiningNode.GetConnectedNodes(pinMembername, PinMode.Output).Any();
    }
}
