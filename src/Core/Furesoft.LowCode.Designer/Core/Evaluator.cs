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

    public SignalStorage Signals { get; set; } = new();

    public async Task Execute(CancellationToken cancellationToken)
    {
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        entryNode.DefiningNode.Drawing = _drawing;
        entryNode.DefiningNode._evaluator = this;
        entryNode.DefiningNode._evaluator.Debugger.CurrentNode = entryNode.DefiningNode;

        foreach (var signal in GetSignals())
        {
            signal.Drawing = _drawing;
            signal._evaluator = this;
            signal._evaluator.Debugger.CurrentNode = entryNode.DefiningNode;
            Signals.Register(signal.Signal, signal);
        }

        try
        {
            await entryNode.DefiningNode.Execute(cancellationToken);
        }
        catch (TaskCanceledException) { }
    }

    public IReadOnlyList<SignalNode> GetSignals()
    {
        return _drawing.Nodes.OfType<CustomNodeViewModel>().Select(_ => _.DefiningNode)
            .OfType<SignalNode>().ToList();
    }

    public T Evaluate<T>(string src)
    {
        return Context.Eval(src).As<T>();
    }
}
