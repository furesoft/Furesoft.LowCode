using System.Collections.Generic;
using System.Linq;
using NodeEditor.Model;
using NodeEditorDemo.Core.Components.ViewModels;

namespace NodeEditorDemo.Core;

public class Evaluator
{
    private readonly IDrawingNode _drawing;
    private readonly Dictionary<string, object> _variables = new();

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
    }
    
    public void Evaluate()
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode.Evaluator = this;
        entryNode.DefiningNode.Evaluate();
    }
}
