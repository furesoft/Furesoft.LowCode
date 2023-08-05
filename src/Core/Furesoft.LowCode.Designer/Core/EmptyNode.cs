using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using Avalonia.Controls;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Designer.ViewModels;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Designer.Core;

[DataContract(IsReference = true)]
public abstract partial class EmptyNode : ViewModelBase, ICustomTypeDescriptor
{
    private string _description;
    internal Evaluator _evaluator;
    private string _label;

    protected EmptyNode(string label)
    {
        Label = label;
    }

    [Browsable(false)] public Context Context { get; private set; }

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
    ///     Gets the previously executed node
    /// </summary>
    [Browsable(false)]
    public EmptyNode PreviousNode { get; set; }

    [Browsable(false)] public bool HasBreakPoint => _evaluator?.Debugger.BreakPointNodes.Contains(this) ?? false;

    public abstract Task Execute(CancellationToken cancellationToken);

    protected async Task ContinueWith(IOutputPin pin, Context context = null,
        CancellationToken cancellationToken = default,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        _evaluator.Debugger.ResetWait();

        var nodes = GetConnectedNodes(pinMembername, PinMode.Output);

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

            try
            {
                await node?.Execute(cancellationToken);
            }
            catch (OutVariableException)
            {
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    protected IEnumerable<EmptyNode> GetInputs(IInputPin pin,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input);
    }

    protected EmptyNode GetInput(IInputPin pin,
        [CallerArgumentExpression("pin")] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Input).First();
    }

    private IReadOnlyList<EmptyNode> GetConnectedNodes(string pinMembername, PinMode mode)
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
        return Context.Eval(StripToJsString<T>(src)).As<T>();
    }

    private string StripToJsString<T>(string src)
    {
        if (typeof(T) == typeof(string) && src.StartsWith('"') && src.EndsWith('"'))
        {
            return src;
        }

        return string.Format("{0}{1}{0}", '"', src.Replace(@"\", @"\\"));
    }

    protected void SetOutVariable(string name, object value)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        Context.GetVariable(name).Assign(value);
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

    public Control GetNodeView(ref double width, ref double height)
    {
        Control nodeView = new DefaultNodeView();

        var nodeViewAttribute = this.GetAttribute<NodeViewAttribute>();
        if (nodeViewAttribute != null)
        {
            nodeView = (Control)Activator.CreateInstance(nodeViewAttribute.Type);

            if (nodeView!.MinHeight > height)
            {
                height = nodeView.MinHeight;
            }

            if (nodeView.MinWidth > width)
            {
                width = nodeView.MinWidth;
            }

            nodeView.Tag = nodeViewAttribute.Parameter;
        }

        return nodeView;
    }

    public virtual void OnInit()
    {
        
    }
}
