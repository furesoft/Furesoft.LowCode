using System.Linq;
using NiL.JS.Core;
using NiL.JS.Extensions;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.ViewModels;

namespace NodeEditorDemo.Core;

public class Evaluator
{
    private readonly IDrawingNode _drawing;
    public Context Context;

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
        Context = new();
    }
    
    public async void Execute()
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode._evaluator = this;
        
        await entryNode.DefiningNode.Execute();
    }

    public T Evaluate<T>(string src)
    {
        return Context.Eval(src).As<T>();
    }
}
