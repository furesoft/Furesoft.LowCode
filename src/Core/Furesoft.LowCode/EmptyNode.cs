using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Designer.ViewModels;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;
using Newtonsoft.Json;
using NiL.JS.Core;
using NiL.JS.Extensions;
using ExecutionMode = Furesoft.LowCode.Evaluation.ExecutionMode;

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

    [Browsable(false)] [JsonIgnore] public ProgressReporter Progress { get; set; } = new();

    [Browsable(false)] public Context Context { get; internal set; }

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

    [Browsable(false)] public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.Graph;

    protected void ApplyPipe<T>()
    {
        //ToDo: convert to pipe js call
        EmptyNode previous = null;

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

    protected Exception CreateError(string msg)
    {
        return new GraphException(new(msg), this);
    }

    protected Exception CreateError<TException>(string msg)
        where TException : Exception
    {
        return new GraphException((Exception)Activator.CreateInstance(typeof(TException), msg), this);
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

    public virtual void Compile(CodeWriter builder)
    {
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

    protected void ReportProgress(int percent, string message)
    {
        Progress.Progress = percent;
        Progress.Message = message;
    }

    protected void CompilePin(IOutputPin pin, CodeWriter builder,
        [CallerArgumentExpression(nameof(pin))]
        string pinMembername = null)
    {
        CompilePin(pinMembername, builder);
    }

    internal void CompilePin(string pinName, CodeWriter builder)
    {
        var nodes = GetConnectedNodes(pinName, PinMode.Output);

        foreach (var node in nodes)
        {
            node.Drawing = Drawing;

            node.Compile(builder);

            if (node is OutputNode)
            {
                node.CompilePin("OutputPin", builder);
            }
        }
    }

    protected bool HasConnection(IOutputPin pin, [CallerArgumentExpression(nameof(pin))] string pinMembername = null)
    {
        return GetConnectedNodes(pinMembername, PinMode.Output).Any();
    }
}
