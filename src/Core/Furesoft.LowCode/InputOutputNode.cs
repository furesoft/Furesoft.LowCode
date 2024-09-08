using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode;

public abstract class InputOutputNode(string label) : OutputNode(label)
{
    [Pin("Flow", PinAlignment.Top)] public IInputPin InputPin { get; set; }
}
