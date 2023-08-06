using NiL.JS.Core;

namespace Furesoft.LowCode.Designer.Debugging;

public class Debugger
{
    private readonly Context _context;
    private TaskCompletionSource _waitTaskSource;

    public Debugger(Context context)
    {
        _context = context;
    }

    public EmptyNode CurrentNode { get; set; }

    public List<EmptyNode> BreakPointNodes { get; set; } = new();

    public bool IsAttached { get; set; }
    public Task WaitTask => _waitTaskSource?.Task;

    public Task Step()
    {
        _waitTaskSource.SetResult();

        return Task.CompletedTask;
    }

    public Task Continue()
    {
        //ToDo: Continue needs to be implemented
        return Task.CompletedTask;
    }

    public DebuggerData GetData()
    {
        var data = new DebuggerData
        {
            CallStack = CurrentNode.GetCallStack(), Locals = new(GetLocalsFromContext(), CurrentNode.GetType())
        };

        return data;
    }

    public object Evaluate(string src)
    {
        return _context.Eval(src).Value;
    }

    internal void ResetWait()
    {
        if (!IsAttached)
        {
            return;
        }

        _waitTaskSource = new();
    }

    private Dictionary<string, object> GetLocalsFromContext()
    {
        return _context.ToDictionary(local => local, local => _context.GetVariable(local).Value);
    }
}
