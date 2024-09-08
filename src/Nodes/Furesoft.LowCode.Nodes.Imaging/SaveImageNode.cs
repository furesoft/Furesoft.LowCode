using SixLabors.ImageSharp.Formats.Jpeg;

namespace Furesoft.LowCode.Nodes.Imaging;

public class SaveImageNode() : ImageNode("Save Image")
{
    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var img = GetImage();
        await img.SaveAsync(File.OpenWrite((string)Filename), new JpegEncoder(), cancellationToken);
    }
}
