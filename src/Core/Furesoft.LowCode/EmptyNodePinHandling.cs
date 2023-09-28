using System.Reflection;
using System.Runtime.CompilerServices;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Designer;

namespace Furesoft.LowCode;

public partial class EmptyNode
{
    private readonly Dictionary<(string, PinMode), IReadOnlyList<EmptyNode>> _connectedNodesCache = new();

    public IEnumerable<EmptyNode> GetInputs(IInputPin pin,
        [CallerArgumentExpression(nameof(pin))]
        string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input);
    }

    public IEnumerable<EmptyNode> GetOutputs(IOutputPin pin,
        [CallerArgumentExpression(nameof(pin))]
        string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Output);
    }

    protected void CompileReadCall(CodeWriter builder, string name, string function, params object[] args)
    {
        builder.AppendKeyword("let").AppendIdentifier(name).AppendSymbol('=');
        builder.AppendCall(function, args).AppendSymbol(';');
    }

    protected void CompileWriteCall(CodeWriter builder, string function, params object[] args)
    {
        builder.AppendCall(function, args).AppendSymbol(';');
    }

    public IEnumerable<string> GetPinNames(PinMode mode)
    {
        var pinType = mode == PinMode.Input ? typeof(IInputPin) : typeof(IOutputPin);

        return from p in GetType().GetProperties()
            where p.PropertyType == pinType
            select p.Name;
    }

    public EmptyNode GetInput(IInputPin pin,
        [CallerArgumentExpression(nameof(pin))]
        string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input)[0];
    }

    public IReadOnlyList<EmptyNode> GetConnectedNodes(string pinMembername, PinMode mode)
    {
        var result = new List<EmptyNode>();
        var pinName = GetPinName(pinMembername);
        var cacheKey = (pinMembername, mode);

        if (_connectedNodesCache.TryGetValue(cacheKey, out var nodes))
        {
            return nodes;
        }

        var connections = GetConnections();
        var pinViewModel = GetPinViewModel(pinName, mode);

        var pinConnections = GetPinConnections(connections, pinViewModel);

        foreach (var pinConnection in pinConnections)
        {
            InitNextNode(pinConnection, pinName, out var parent, mode);

            result.Add(parent);
        }

        _connectedNodesCache.Add(cacheKey, result);

        return result;
    }

    // ReSharper disable once FlagArgument
    private void InitNextNode(IConnector pinConnection, string pinName, out EmptyNode parentNode, PinMode mode)
    {
        CustomNodeViewModel parent;

        if (pinConnection.Start.Name == pinName && pinConnection.Start.Mode == mode)
        {
            parent = pinConnection.End.Parent as CustomNodeViewModel;
        }
        else if (pinConnection.End.Name == pinName && pinConnection.End.Mode == mode)
        {
            parent = pinConnection.Start.Parent as CustomNodeViewModel;
        }
        else
        {
            parentNode = null;
            return;
        }

        parentNode = parent?.DefiningNode;
    }

    private static IEnumerable<IConnector> GetPinConnections(IEnumerable<IConnector> connections, IPin pinViewModel)
    {
        return from conn in connections
            where conn.Start == pinViewModel || conn.End == pinViewModel
            select conn;
    }

    private IPin GetPinViewModel(string pinName, PinMode mode)
    {
        return (from node in Drawing.Nodes
                where ((CustomNodeViewModel)node).DefiningNode == this
                from pinn in node.Pins
                where pinn.Name == pinName
                select pinn)
            .OfType<PinViewModel>()
            .FirstOrDefault(_ => _.Mode == mode);
    }

    private IEnumerable<IConnector> GetConnections()
    {
        return from connection in Drawing.Connectors
            where ((CustomNodeViewModel)connection.Start.Parent).DefiningNode == this
                  || ((CustomNodeViewModel)connection.End.Parent).DefiningNode == this
            select connection;
    }

    private string GetPinName(string propertyName)
    {
        propertyName = StripPinName(propertyName);

        var propInfo = GetType().GetProperty(propertyName);

        var attr = propInfo!.GetCustomAttribute<PinAttribute>();

        return attr == null ? propInfo!.Name : attr.Name;
    }

    private string StripPinName(string propertyName)
    {
        return propertyName.Contains('.')
            ? propertyName.Split(".")[^1]
            : propertyName;
    }
}
