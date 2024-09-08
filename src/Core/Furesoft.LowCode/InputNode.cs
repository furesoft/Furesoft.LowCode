using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode;

public abstract class InputNode(string label) : EmptyNode(label)
{
    [Pin("Flow", PinAlignment.Top)] public IInputPin InputPin { get; set; }
}
