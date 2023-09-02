using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.ViewModels;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;
using Newtonsoft.Json;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode;

[DataContract(IsReference = true)]
[GraphAnalyzer(typeof(GenericNodeAnalyzer))]
public abstract partial class EmptyNode : ViewModelBase, ICustomTypeDescriptor
{
    private string _description;
    internal Evaluator _evaluator;
    private string _label;
    private bool _showDescription;
    public OptionsProvider Options;

    protected EmptyNode(string label)
    {
        Label = label;
        ID = Guid.NewGuid();
    }

    [Browsable(false)] public Context Context { get; private set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
    [JsonIgnore]
    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    [Browsable(false)]
    [DataMember(EmitDefaultValue = false)]
    public Guid ID { get; set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
    [JsonIgnore]
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public bool ShowDescription
    {
        get => _showDescription;
        set => SetProperty(ref _showDescription, value);
    }

    /// <summary>
    ///     Gets the previously executed node
    /// </summary>
    [Browsable(false)]
    public EmptyNode PreviousNode { get; set; }

    protected void ApplyPipe<T>()
    {
        var previous = GetPreviousNode<InputOutputNode>();

        switch (previous)
        {
            case IPipeable prevPipe when this is IPipeable pipe:
                pipe.PipeVariable = prevPipe.PipeVariable;

                break;

            case IOutVariableProvider outVariableProvider:
                var pip = Evaluate<object>(outVariableProvider);

                if (pip is T pipes && this is IPipeable po)
                {
                    po.PipeVariable = pipes;
                }

                break;
        }
    }

    public abstract Task Execute(CancellationToken cancellationToken);

    protected async Task ContinueWith(IOutputPin pin, CancellationToken cancellationToken, Context context = null,
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

            await InitAndExecuteNode(context, cancellationToken, node);

            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private async Task InitAndExecuteNode(Context context, CancellationToken cancellationToken, EmptyNode node)
    {
        node._evaluator = _evaluator;
        node.Context = context ?? _evaluator.Context;
        node.Drawing = Drawing;
        node.PreviousNode = this;
        node._evaluator.Debugger.CurrentNode = node;
        node.Options = _evaluator.Options;

        await node?.Execute(cancellationToken);
    }

    protected Exception CreateError(string msg)
    {
        return new GraphException(new(msg), this);
    }

    protected Exception CreateError<TException>(string msg)
        where TException : Exception
    {
        return new GraphException((Exception)Activator.CreateInstance(typeof(TException), msg), this);
    }

    protected T GetPreviousNode<T>()
        where T : EmptyNode
    {
        if (PreviousNode is T node)
        {
            return node;
        }

        return PreviousNode?.GetPreviousNode<T>();
    }

    protected T Evaluate<T>(Evaluatable<T> src)
    {
        return Context.Eval(src.Source).As<T>();
    }

    protected T Evaluate<T>(IOutVariableProvider src)
    {
        return Context.Eval(src.OutVariable).As<T>();
    }

    protected void SetOutVariable(string name, object value)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        Context.GetVariable(name).Assign(value, Context);
    }

    protected void DeleteConstant(string name)
    {
        Context.GetVariable(name).Assign(JSValue.NotExists);
    }

    protected void DefineConstant(string name, object value, Context context = null)
    {
        (context ?? Context).DefineVariable(name)
            .Assign(value);
    }

    public Control GetView(ref double width, ref double height)
    {
        if (AppContext.GetData("DesignerMode") is false)
        {
            return null;
        }

        Control nodeView = new DefaultNodeView();

        var nodeViewAttribute = GetAttribute<NodeViewAttribute>();

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

    public T GetAttribute<T>()
        where T : Attribute
    {
        return TypeDescriptor.GetAttributes(this)
            .OfType<T>().FirstOrDefault();
    }

    protected void ReportProgress(byte percent, string message)
    {
        _evaluator.Progress?.Report(percent, message);
    }
}
