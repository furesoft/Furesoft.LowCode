using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;
using NiL.JS.Extensions;

namespace Furesoft.LowCode.Nodes.Imaging;

[NodeCategory("Imaging")]
[NodeIcon(
    "M42.7916 78.6666H106.7916C112.6829 78.6666 117.4583 73.8911 117.4583 68V4C117.4583-1.891 112.6828-6.6666 106.7916-6.6666H42.7916C36.9006-6.6666 32.125-1.891 32.125 4V68C32.125 73.8912 36.9006 78.6666 42.7916 78.6666ZM42.7916 68V4H106.7916V68H42.7916ZM10.7916 25.3333H21.4582V89.3333H85.4582V99.9999H21.4582C15.5672 99.9999 10.7916 95.2244 10.7916 89.3333V25.3333ZM53.4584 57.3332H101.4584L81.4584 28.2963 69.4584 44.889 65.4584 40.7407 53.4584 57.3332ZM57.4584 26.2223C57.4584 29.6586 60.1447 32.4444 63.4584 32.4444S69.4584 29.6586 69.4584 26.2223C69.4584 22.7858 66.772 20 63.4584 20S57.4584 22.7858 57.4584 26.2223Z")]
public abstract class ImageNode : InputOutputNode, IPipeable
{
    protected ImageNode(string label) : base(label)
    {
    }

    [DataMember(EmitDefaultValue = false)] public string ImageName { get; set; }

    [DataMember(EmitDefaultValue = false)] public Evaluatable<string> Filename { get; set; }

    public object PipeVariable { get; set; }

    protected Image GetImage()
    {
        if (PipeVariable is Image pipedImage)
        {
            return pipedImage;
        }

        var value = Context.GetVariable(ImageName);

        return value.As<Image>();
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);

    public override async Task Execute(CancellationToken cancellationToken)
    {
        ApplyPipe<Image>();

        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken);
    }
}
