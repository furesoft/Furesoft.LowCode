using Furesoft.LowCode.Compilation;
using MsBox.Avalonia;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.Import("outputWriter", DebugOutNode.OutputWriter);
        context.Import("showMessageBox", ShowMessageBox);
    }

    public static async void ShowMessageBox(string title, string message)
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard(title, message);

        await box.ShowWindowAsync();
    }
}
