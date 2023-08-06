using System.Reflection;
using System.Runtime.CompilerServices;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Designer.Core;

public partial class EmptyNode
{
    public IEnumerable<EmptyNode> GetInputs(IInputPin pin,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input);
    }
    
    public IEnumerable<EmptyNode> GetOutputs(IOutputPin pin,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Output);
    }

    public EmptyNode GetInput(IInputPin pin,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input)[0];
    }

    public IReadOnlyList<EmptyNode> GetConnectedNodes(string pinMembername, PinMode mode)
    {
        var result = new List<EmptyNode>();
        var pinName = GetPinName(pinMembername);
        var connections = GetConnections();
        var pinViewModel = GetPinViewModel(pinName, mode);

        var pinConnections = GetPinConnections(connections, pinViewModel);

        foreach (var pinConnection in pinConnections)
        {
            InitNextNode(pinConnection, pinName, out var parent, mode);

            result.Add(parent);
        }

        return result;
    }
    
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

        parentNode = parent.DefiningNode;
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
            select pinn).OfType<PinViewModel>().FirstOrDefault(_ => _.Mode == mode);
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

        var attr = propInfo.GetCustomAttribute<PinAttribute>();

        if (attr == null)
        {
            return propInfo.Name;
        }

        return attr.Name;
    }

    private string StripPinName(string propertyName)
    {
        if (!propertyName.Contains("."))
        {
            return propertyName;
        }

        return propertyName.Split(".")[^1];
    }
}
