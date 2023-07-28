﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Furesoft.LowCode.Designer.Core.Components.ViewModels;
using Furesoft.LowCode.Editor.Model;
using NiL.JS.Core;
using NiL.JS.Extensions;
using Debugger = Furesoft.LowCode.Designer.Core.Debugging.Debugger;

namespace Furesoft.LowCode.Designer.Core;

public class Evaluator
{
    private readonly IDrawingNode _drawing;
    public Context Context;
    internal Debugger Debugger { get; }

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
        Context = new();
        Debugger = new(Context);
    }
    
    public async Task Execute(CancellationToken cancellationToken)
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode._evaluator = this;
        entryNode.DefiningNode._evaluator.Debugger.CurrentNode = entryNode.DefiningNode;

        try
        {
            await entryNode.DefiningNode.Execute(cancellationToken: cancellationToken);
        }
        catch (TaskCanceledException ex){}
    }

    public T Evaluate<T>(string src)
    {
        return Context.Eval(src).As<T>();
    }
}