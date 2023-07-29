using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.ViewModels;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Designer.Core;

[DataContract(IsReference = true)]
public abstract partial class VisualNode : ViewModelBase, ICustomTypeDescriptor
{
    private string _label;
    private string _description;
    internal Evaluator _evaluator;
    protected Context Context { get; private set; }

    public VisualNode(string label)
    {
        Label = label;
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// Gets the previously executed node
    /// </summary>
    [Browsable(false)]
    public VisualNode PreviousNode { get; set; }

    public abstract Task Execute(CancellationToken cancellationToken);

    protected async Task ContinueWith(IOutputPin pin, Context context = null,
        CancellationToken cancellationToken = default,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        _evaluator.Debugger.ResetWait();

        var nodes = GetConnectedNodes(pinMembername, PinMode.Output, context);

        foreach (var node in nodes)
        {
            if (_evaluator.Debugger.IsAttached)
            {
                await _evaluator.Debugger.WaitTask;
            }

            node._evaluator = _evaluator;
            node.Context = context ?? _evaluator.Context;
            node.Drawing = Drawing;
            node.PreviousNode = this;
            node._evaluator.Debugger.CurrentNode = node;

            await node?.Execute(cancellationToken);
            
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    protected IEnumerable<object> GetInputs(IInputPin pin,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input);
    }

    private IEnumerable<VisualNode> GetConnectedNodes(string pinMembername, PinMode mode, Context context = null)
    {
        var pinName = GetPinName(pinMembername);
        var connections = GetConnections();
        var pinViewModel = GetPinViewModel(pinName, mode);

        var pinConnections = GetPinConnections(connections, pinViewModel);

        foreach (var pinConnection in pinConnections)
        {
            InitNextNode(pinConnection, pinName, out var parent);

            yield return parent;
        }
    }

    private void InitNextNode(IConnector pinConnection, string pinName, out VisualNode parentNode)
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
            parentNode = null;
            return;
        }

        parentNode = parent.DefiningNode;
    }

    [Browsable(false)]
    public bool HasBreakPoint
    {
        get
        {
            return _evaluator?.Debugger.BreakPointNodes.Contains(this) ?? false;
        }
    }

    public void AddBreakPoint()
    {
        _evaluator.Debugger.BreakPointNodes.Add(this);
    }

    public void RemoveBreakPoint()
    {
        _evaluator.Debugger.BreakPointNodes.Remove(this);
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
        return Context.Eval(src).As<T>();
    }

    protected void SetOutVariable(string name, object value)
    {
        Context.GetVariable(name).Assign(JSValue.Wrap(value));
    }

    protected void DefineConstant(string name, object value, Context context = null)
    {
        (context ?? Context).DefineVariable(name).Assign(JSValue.Wrap(value));
    }

    public string GetCallStack()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{Label}:");
        foreach (PropertyDescriptor value in GetProperties())
        {
            sb.AppendLine($"\t{value.Name}: {value.GetValue(this)}");
        }

        sb.AppendLine(PreviousNode?.GetCallStack());

        return sb.ToString();
    }
}
