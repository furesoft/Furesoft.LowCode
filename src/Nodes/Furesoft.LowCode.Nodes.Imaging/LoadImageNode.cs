using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.Imaging;

public class LoadImageNode : ImageNode
{
    public LoadImageNode() : base("Load Image")
    {
    }

    public override void Compile(CodeWriter builder)
    {
        CompileReadCall(builder, ImageName, "Image.Load", Filename);
    }
}
