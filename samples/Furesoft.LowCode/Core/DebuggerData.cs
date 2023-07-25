using System.Collections.Generic;

namespace Furesoft.LowCode.Core;

public class DebuggerData
{
    public string CallStack { get; set; }
    public Dictionary<string, object> Locals { get; set; } = new();
}
