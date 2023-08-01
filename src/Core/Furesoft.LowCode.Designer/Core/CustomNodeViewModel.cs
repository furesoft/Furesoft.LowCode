using Furesoft.LowCode.Editor.MVVM;

namespace Furesoft.LowCode.Designer.Core;

public class CustomNodeViewModel : NodeViewModel
{
    public bool IsRemovable { get; set; } = true;
    public bool IsMovable { get; set; } = true;

    public string Category { get; set; }

    public EmptyNode DefiningNode { get; set; }

    public override bool CanRemove()
    {
        return IsRemovable;
    }

    public override bool CanMove()
    {
        return IsMovable;
    }
}
