using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;
using Timer = System.Timers.Timer;

namespace Furesoft.LowCode.Nodes.Scheduling;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
    }

    [JavaScriptName("setInterval")]
    public static void SetInterval(double interval, ICallable callback)
    {
        var timer = new Timer();
        timer.Interval = interval;
        timer.Elapsed += (sender, args) =>
        {
            callback.Call(null, new Arguments());
        };
        timer.Start();
    }

    [JavaScriptName("delay")]
    public static void Delay(int milliseconds)
    {
        Task.Delay(milliseconds).GetAwaiter().GetResult();
    }
}
