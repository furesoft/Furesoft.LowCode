using Furesoft.LowCode.Compilation;
using MsBox.Avalonia;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstant("outputWriter", context.GlobalContext.ProxyValue(DebugOutNode.OutputWriter));
        context.DefineConstant("showMessageBox", context.GlobalContext.ProxyValue(ShowMessageBox));
    }

    public static async void ShowMessageBox(string title, string message)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message);

        await box.ShowWindowAsync();
    }
}
