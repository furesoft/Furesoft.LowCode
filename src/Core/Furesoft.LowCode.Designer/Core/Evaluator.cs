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

        var entryNode = _drawing.GetNodes<EntryNode>().First().DefiningNode;

        InitNode(entryNode);

        foreach (var signal in GetSignals())
        {
            InitNode(signal);

            Signals.Register(signal.Signal, signal);
        }

        try
        {
            await entryNode.Execute(cancellationToken);
        }
        catch (TaskCanceledException) { }
    }

    private void InitCredentils()
    {
        foreach (var credentialName in CredentialStorage.GetKeys())
        {
            var wrapValue = Context.GlobalContext.WrapValue(CredentialStorage.Get(credentialName));

            Context.GlobalContext.DefineConstant(credentialName, wrapValue);
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
        return _drawing.GetNodes<SignalNode>()
            .Select(node => node.DefiningNode)
            .OfType<SignalNode>()
            .ToList();
    }

    public T Evaluate<T>(string src)
    {
        return Context.Eval(src).As<T>();
    }
}
