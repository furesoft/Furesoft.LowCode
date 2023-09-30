using Furesoft.LowCode.Compilation;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.Imaging;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstructor(typeof(Image));
    }
}
