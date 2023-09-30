using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.Imaging;

public class SaveImageNode : ImageNode
{
    public SaveImageNode() : base("Save Image")
    {
    }

    public override void Compile(CodeWriter builder)
    {
        builder.AppendCall(ImageName + ".Save", Filename);
    }
}
