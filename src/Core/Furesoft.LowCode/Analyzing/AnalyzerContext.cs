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

    //ToDo: Figure out from which direction the node comes in dependency of the current node
    
    //bool IsOutputConnected<Type>(bool indirection)
    //bool IsInputConnected<Type>(bool indirection)

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
