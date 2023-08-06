namespace Furesoft.LowCode;

public abstract class OutputNode : EmptyNode
{
    protected OutputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Bottom)] public IOutputPin OutputPin { get; set; }
}
