namespace Furesoft.LowCode.Designer.Core;

public abstract class OutputNode : EmptyNode
{
    protected OutputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Bottom)] public IOutputPin OutputPin { get; set; }
}
