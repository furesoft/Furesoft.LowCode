using Furesoft.LowCode.Compilation;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.IO;

public class ScriptInitalizer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstructor(typeof(System.Console));
    }
}
