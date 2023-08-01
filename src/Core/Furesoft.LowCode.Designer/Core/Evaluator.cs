using Furesoft.LowCode.Designer.Core.Components.ViewModels;
using Furesoft.LowCode.Designer.Core.Debugging;
using Furesoft.LowCode.Editor.Model;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Designer.Core;

public class Evaluator
{
    private readonly IDrawingNode _drawing;
    public Context Context;

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
        Context = new();
        Debugger = new(Context);
    }

    internal Debugger Debugger { get; }

    public static SignalStorage Signals { get; set; } = new();

    public async Task Execute(CancellationToken cancellationToken)
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode._evaluator = this;
        entryNode.DefiningNode._evaluator.Debugger.CurrentNode = entryNode.DefiningNode;

        try
        {
            await entryNode.DefiningNode.Execute(cancellationToken);
        }
        catch (TaskCanceledException) { }
    }

    public T Evaluate<T>(string src)
    {
        return Context.Eval(src).As<T>();
    }
}
