using System.ComponentModel;

namespace Furesoft.LowCode;

public partial class EmptyNode
{
    [Browsable(false)] public bool HasBreakPoint => _evaluator?.Debugger.BreakPointNodes.Contains(this) ?? false;

    public void AddBreakPoint()
    {
        _evaluator.Debugger.BreakPointNodes.Add(this);
    }

    public void RemoveBreakPoint()
    {
        _evaluator.Debugger.BreakPointNodes.Remove(this);
    }
}
