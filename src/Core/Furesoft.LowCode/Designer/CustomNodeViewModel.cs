using Furesoft.LowCode.Analyzing;

namespace Furesoft.LowCode.Designer;

public class CustomNodeViewModel : NodeViewModel
{
    public AnalyzerContext AnalyzerContext { get; }

    public CustomNodeViewModel()
    {
        AnalyzerContext = new(this);
    }

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
