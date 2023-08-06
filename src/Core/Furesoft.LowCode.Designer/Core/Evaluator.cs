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
        CredentialStorage = new IsolatedCredentailStorage();
    }

    internal Debugger Debugger { get; }

    public SignalStorage Signals { get; } = new();
    public ICredentialStorage CredentialStorage { get; set; }

    public async Task Execute(CancellationToken cancellationToken)
    {
        InitCredentils();
        
        var entryNode = _drawing.Nodes.OfType<CustomNodeViewModel>()
            .First(node => node.DefiningNode.GetType() == typeof(EntryNode));

        InitNode(entryNode.DefiningNode);

        foreach (var signal in GetSignals())
        {
            InitNode(signal);

            Signals.Register(signal.Signal, signal);
        }

        try
        {
            await entryNode.DefiningNode.Execute(cancellationToken);
        }
        catch (TaskCanceledException) { }
    }

    private void InitCredentils()
    {
        foreach (var credentialName in CredentialStorage.GetKeys())
        {
            Context.GlobalContext.DefineConstant(credentialName, JSValue.Wrap(CredentialStorage.Get(credentialName)));
        }
    }

    private void InitNode(EmptyNode node)
    {
        node.Drawing = _drawing;
        node._evaluator = this;
        node._evaluator.Debugger.CurrentNode = node;

        node.OnInit();
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
