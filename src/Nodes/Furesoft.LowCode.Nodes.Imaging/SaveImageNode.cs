namespace Furesoft.LowCode.Nodes.Imaging;

public class SaveImageNode : ImageNode
{
    public SaveImageNode() : base("Save Image")
    {
    }


    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var img = GetImage();
        await img.SaveAsync(Filename, cancellationToken);
    }
}
