﻿using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Furesoft.LowCode.Core.Components.ViewModels;
using NiL.JS.Core;
using NiL.JS.Extensions;
using NodeEditor.Model;
using Debugger = Furesoft.LowCode.Core.Debugging.Debugger;

namespace Furesoft.LowCode.Core;

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
    
    public async Task Execute()
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode._evaluator = this;
        entryNode.DefiningNode._evaluator.Debugger.CurrentNode = entryNode.DefiningNode;
        
        await entryNode.DefiningNode.Execute();
    }

    public T Evaluate<T>(string src)
    {
        return Context.Eval(src).As<T>();
    }
}
