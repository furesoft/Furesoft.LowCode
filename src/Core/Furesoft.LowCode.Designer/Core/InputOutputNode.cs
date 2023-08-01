namespace Furesoft.LowCode.Designer.Core;

public abstract class InputOutputNode : OutputNode
{
    protected InputOutputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Top)] public IInputPin InputPin { get; set; }
}
