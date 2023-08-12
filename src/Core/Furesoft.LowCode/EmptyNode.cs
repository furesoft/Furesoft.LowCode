﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Furesoft.LowCode.Analyzing;
using Furesoft.LowCode.Designer.ViewModels;
using Furesoft.LowCode.Nodes.Analyzers;
using Furesoft.LowCode.NodeViews;
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

    protected EmptyNode(string label)
    {
        Label = label;
        ID = Guid.NewGuid();
    }

    [Browsable(false)] public Context Context { get; private set; }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Browsable(false)]
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

    protected T Evaluate<T>(Evaluatable src)
    {
        return Context.Eval(src.Source).As<T>();
    }

    protected void SetOutVariable(string name, object value)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        Context.GetVariable(name).Assign(value);
    }

    protected void DeleteConstant(string name)
    {
        Context.GetVariable(name).Assign(JSValue.Undefined);
    }

    protected void DefineConstant(string name, object value, Context context = null)
    {
        (context ?? Context).DefineVariable(name).Assign(Context.GlobalContext.WrapValue(value));
    }

    public Control GetView(ref double width, ref double height)
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
