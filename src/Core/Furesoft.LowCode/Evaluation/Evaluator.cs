using System.Diagnostics;
using System.Text;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Designer.Services.Serializing;
using Furesoft.LowCode.Reporters;
using NiL.JS.Core;
using Debugger = Furesoft.LowCode.Designer.Debugging.Debugger;

namespace Furesoft.LowCode.Evaluation;

public class Evaluator : IEvaluator
{
    private readonly IDrawingNode _drawing;
    public Context Context;
    public OptionsProvider Options;

    public Evaluator(IDrawingNode drawing)
    {
        _drawing = drawing;
        Init();

        SetDesignerMode(true);
    }

    public Evaluator(Project project, bool designerMode)
    {
        var mainGraphNode = project.GetMainGraph();

        _drawing = mainGraphNode;
        Init();

        SetDesignerMode(designerMode);
    }

    public Evaluator(Stream strm)
    {
        SetDesignerMode(false);

        var serializer = new NodeSerializer(typeof(ObservableCollection<>));
        using var streamReader = new StreamReader(strm, Encoding.UTF8);
        var text = streamReader.ReadToEnd();

        _drawing = serializer.Deserialize<DrawingNodeViewModel>(text);

        Init();
    }

    public Evaluator(string text)
    {
        SetDesignerMode(false);

        var serializer = new NodeSerializer(typeof(ObservableCollection<>));

        _drawing = serializer.Deserialize<DrawingNodeViewModel>(text);

        Init();
    }

    internal Debugger Debugger { get; set; }

    public SignalStorage Signals { get; } = new();
    public ICredentialStorage CredentialStorage { get; set; }

    public async Task Execute(CancellationToken cancellationToken)
    {
        InitCredentials();

        var entryNode = _drawing.GetNodes<EntryNode>().First().DefiningNode;

        InitNode(entryNode);

        var compiler = new GraphCompiler();
        var compiledGraphSource = compiler.Compile(entryNode);
        Debug.WriteLine("Compiled Graph: " + compiledGraphSource);

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

    private static void SetDesignerMode(bool value)
    {
        AppContext.SetData("DesignerMode", value);
    }

    private void Init()
    {
        Context = new();
        Debugger = new(Context);
        CredentialStorage = new IsolatedCredentailStorage();
    }

    private void InitCredentials()
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

        if (AppContext.GetData("DesignerMode") is false)
        {
            node.Progress = new ConsoleProgressReporter();
        }

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
