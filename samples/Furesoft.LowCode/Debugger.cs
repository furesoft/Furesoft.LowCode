using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Furesoft.LowCode.Core;
using NiL.JS.Core;

namespace Furesoft.LowCode;

public class Debugger
{
    private Context Context;
    public VisualNode CurrentNode;

    public Debugger(Context context)
    {
        Context = context;
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

    private Dictionary<string, object> GetLocalsFromContext()
    {
        return Context.ToDictionary(local => local, local => Context.GetVariable(local).Value);
    }
}

public class DebuggerData
{
    public string CallStack { get; set; }
    public Dictionary<string, object> Locals { get; set; } = new();
}
