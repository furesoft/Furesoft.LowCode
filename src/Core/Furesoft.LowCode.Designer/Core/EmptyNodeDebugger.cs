using System.ComponentModel;
using System.Text;

namespace Furesoft.LowCode.Designer.Core;

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
    
    public string GetCallStack()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{Label}:");
        foreach (PropertyDescriptor value in GetProperties())
        {
            sb.AppendLine($"\t{value.Name}: {value.GetValue(this)}");
        }

        sb.AppendLine(PreviousNode?.GetCallStack());

        return sb.ToString();
    }
}
