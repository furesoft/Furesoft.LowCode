using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiL.JS.Core;

namespace Furesoft.LowCode.Core;

public class Debugger
{
    private readonly Context _context;
    public VisualNode CurrentNode;

    public Debugger(Context context)
    {
        _context = context;
    }

    public Task Step()
    {
        return Task.CompletedTask;
    }

    public Task Continue()
    {
        return Task.CompletedTask;
    }

    public DebuggerData GetData()
    {
        var data = new DebuggerData {CallStack = CurrentNode.GetCallStack(), Locals = GetLocalsFromContext()};

        return data;
    }

    public object Evaluate(string src)
    {
       return _context.Eval(src).Value;
    }

    private Dictionary<string, object> GetLocalsFromContext()
    {
        return _context.ToDictionary(local => local, local => _context.GetVariable(local).Value);
    }
}
