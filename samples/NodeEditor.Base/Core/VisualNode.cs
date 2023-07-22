using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using NodeEditor.Model;
using NodeEditorDemo.Core.NodeBuilding;
using NodeEditorDemo.ViewModels;

namespace NodeEditorDemo.Core;

[DataContract(IsReference = true)]
public abstract class VisualNode : ViewModelBase
{
    private string _label;
    internal Evaluator _evaluator;

    public VisualNode(string label)
    {
        Label = label;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    public abstract Task Execute();

    protected async Task ContinueWith(IOutputPin pin, [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        var pinName = GetPinName(pinMembername);

        var connections = GetConnections();

        var pinViewModel = GetPinViewModel(pinName);

        var pinConnections = GetPinConnections(connections, pinViewModel);

        foreach (var pinConnection in pinConnections)
        {
            CustomNodeViewModel parent;

            if (pinConnection.Start.Name == pinName)
            {
                parent = pinConnection.End.Parent as CustomNodeViewModel;
            }
            else if (pinConnection.End.Name == pinName)
            {
                parent = pinConnection.Start.Parent as CustomNodeViewModel;
            }
            else
            {
                continue;
            }

            parent.DefiningNode._evaluator = _evaluator;
            parent.DefiningNode.Drawing = Drawing;

            await parent.DefiningNode.Execute();
        }
    }

    private static IEnumerable<IConnector> GetPinConnections(IEnumerable<IConnector> connections, IPin pinViewModel)
    {
        return from conn in connections
            where conn.Start == pinViewModel || conn.End == pinViewModel
            select conn;
    }

    private IPin GetPinViewModel(string pinName)
    {
        return (from node in Drawing.Nodes
            where ((CustomNodeViewModel)node).DefiningNode == this
            from pinn in node.Pins
            where pinn.Name == pinName
            select pinn).FirstOrDefault();
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
        var propInfo = GetType().GetProperty(propertyName);

        var attr = propInfo.GetCustomAttribute<PinAttribute>();

        if (attr == null)
        {
            return propInfo.Name;
        }

        return attr.Name;
    }

    protected T Evaluate<T>(string src)
    {
        return _evaluator.Evaluate<T>(src);
    }
}
