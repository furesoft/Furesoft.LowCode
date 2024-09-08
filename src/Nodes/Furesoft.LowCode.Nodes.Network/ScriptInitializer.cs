using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Furesoft.LowCode.Nodes.Network;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.ImportAsObject<ScriptInitializer>("Network");
    }

    [JavaScriptName("getSiteContent")]
    public static string GetSiteContent(string url)
    {
        var client = new HttpClient();
        var content = client.GetStringAsync(url).GetAwaiter().GetResult();

        return content;
    }
}