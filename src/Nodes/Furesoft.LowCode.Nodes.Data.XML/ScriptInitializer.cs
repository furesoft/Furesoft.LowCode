using Furesoft.LowCode.Compilation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Furesoft.LowCode.Nodes.Data.XML;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.ImportAsObject<ScriptInitializer>("XML");
    }

    [JavaScriptName("read")]
    public static void ReadXml(string path, System.Data.DataTable dataTable)
    {
        dataTable.ReadXml(path);
    }

    [JavaScriptName("write")]
    public static void WriteXml(string path, System.Data.DataTable dataTable)
    {
        dataTable.WriteXml(path);
    }
}
