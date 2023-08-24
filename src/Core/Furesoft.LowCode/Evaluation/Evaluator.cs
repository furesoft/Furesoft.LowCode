using System.Text;
using Furesoft.LowCode.Designer.Debugging;
using Furesoft.LowCode.Designer.Services.Serializing;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Nodes;
using NiL.JS.Core;

namespace Furesoft.LowCode.Evaluation;

public class Evaluator
{
    private readonly IDrawingNode _drawing;
    public readonly Context Context;
    public OptionsProvider Options;

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
        Context = new();
        Debugger = new(Context);
        CredentialStorage = new IsolatedCredentailStorage();

        AppContext.SetData("DesignerMode", true);
    }

    public Evaluator(Stream strm)
    {
        AppContext.SetData("DesignerMode", false);

        var serializer = new NodeSerializer(typeof(ObservableCollection<>));
        using var streamReader = new StreamReader(strm, Encoding.UTF8);
        var text = streamReader.ReadToEnd();

        _drawing = serializer.Deserialize<DrawingNodeViewModel>(text);

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

            if (Context.GlobalContext.GetVariable(credentialName) != JSValue.NotExists)
            {
                return;
            }

            Context.GlobalContext.DefineConstant(credentialName, wrapValue);
        }
    }

    private void InitNode(EmptyNode node)
    {
        node.Drawing = _drawing;
        node._evaluator = this;
        node._evaluator.Debugger.CurrentNode = node;
        node.Options = Options;

        node.OnInit();
    }

    public IReadOnlyList<SignalNode> GetSignals()
    {
        return _drawing.GetNodes<SignalNode>()
            .Select(node => node.DefiningNode)
            .OfType<SignalNode>()
            .ToList();
    }
}
