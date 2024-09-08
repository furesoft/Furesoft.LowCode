using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode;

public abstract class OutputNode(string label) : EmptyNode(label)
{
    [Pin("Flow", PinAlignment.Bottom)] public IOutputPin OutputPin { get; set; }
}
