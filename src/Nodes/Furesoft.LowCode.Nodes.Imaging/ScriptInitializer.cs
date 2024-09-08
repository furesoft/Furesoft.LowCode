using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using SixLabors.ImageSharp;

namespace Furesoft.LowCode.Nodes.Imaging;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstructor(typeof(Image));
    }
}
