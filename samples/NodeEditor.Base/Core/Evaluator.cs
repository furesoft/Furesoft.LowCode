using System.Collections.Generic;
using System.Linq;
using NiL.JS;
using NiL.JS.Core;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.ViewModels;

namespace NodeEditorDemo.Core;

public class Evaluator
{
    private readonly IDrawingNode _drawing;
    private Context _context;

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
        _context = new();
    }
    
    public async void Execute()
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode.Evaluator = this;
        
        await entryNode.DefiningNode.Execute();
    }

    public object Evaluate(string src)
    {
        return _context.Eval(src);
    }
}
