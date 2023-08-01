namespace Furesoft.LowCode.Designer.Core;

public abstract class InputOutputNode : EmptyNode
{
    protected InputOutputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Top)] public IInputPin InputPin { get; set; }

    [Pin("Flow", PinAlignment.Bottom)] public IOutputPin OutputPin { get; set; }
}
