using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode;

public abstract class InputOutputNode : OutputNode
{
    protected InputOutputNode(string label) : base(label)
    {
    }

    [Pin("Flow", PinAlignment.Top)] public IInputPin InputPin { get; set; }

    protected void CompileReadCall(CodeWriter builder, string name, string function, params object[] args)
    {
        builder.AppendKeyword("let").AppendIdentifier(name).AppendSymbol('=');
        builder.AppendCall(function, args).AppendSymbol(';');
    }

    protected void CompileWriteCall(CodeWriter builder, string function, params object[] args)
    {
        builder.AppendCall(function, args).AppendSymbol(';');
    }
}
