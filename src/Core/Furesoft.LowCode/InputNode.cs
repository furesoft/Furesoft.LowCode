namespace Furesoft.LowCode;

public abstract class InputNode : EmptyNode
{
    protected InputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Top)] public IInputPin InputPin { get; set; }
}
