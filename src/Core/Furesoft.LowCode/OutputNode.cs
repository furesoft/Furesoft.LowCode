using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode;

public abstract class OutputNode : EmptyNode
{
    protected OutputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Bottom)] public IOutputPin OutputPin { get; set; }

    public override void Compile(CodeWriter builder)
    {
        builder.AppendCall($"executeNode", Drawing.Name, ID);
        CompilePin(OutputPin, builder);
    }
}
