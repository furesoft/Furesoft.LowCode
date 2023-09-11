namespace Furesoft.LowCode.Nodes.Imaging;

public class LoadImageNode : ImageNode
{
    public LoadImageNode() : base("Load Image")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        PipeVariable = Image.Load(Filename);

        if (!string.IsNullOrEmpty(ImageName))
        {
            Context.DefineConstant(ImageName,
                Context.GlobalContext.WrapValue(PipeVariable));
        }

        return Task.CompletedTask;
    }
}
