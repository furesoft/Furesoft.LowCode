using NodeEditor.Mvvm;

namespace Furesoft.LowCode.Core;

public class CustomNodeViewModel : NodeViewModel
{
    public bool IsRemovable { get; set; } = true;
    public bool IsMovable { get; set; } = true;
    
    public VisualNode DefiningNode { get; set; }

    public override bool CanRemove()
    {
        return IsRemovable;
    }

    public override bool CanMove()
    {
        return IsMovable;
    }
}
