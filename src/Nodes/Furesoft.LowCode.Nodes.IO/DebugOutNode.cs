using System.Diagnostics;

namespace Furesoft.LowCode.Nodes.IO;

[DataContract(IsReference = true)]
[Description("Write a text to the debug log")]
[NodeCategory("IO/Debug")]
[NodeView(typeof(IconNodeView),
    "M88 282.9875L89.7375 283.7H266.5L268.25 282.9875L282.975 268.25L283.6875 266.5125V89.75L282.975 88L268.2375000000001 73.275L266.5000000000001 72.5625H175V75A24.875 24.875 0 0 1 168.15 92.1875H264.0625V264.0625H92.1875V173.0375A71.625 71.625 0 0 1 72.55 175.0875V266.5125L73.2625 268.2625L88 282.975zM207.1375 164.8375L172.025 129.8375A25 25 0 0 1 167.675 135.675L162.4375 140.925L190.3875 168.8625L145.3125 215.0375L155.475 225.45L207.1375 173.7875V164.8375zM120.25 114.75L136.75 131.25L150 118L128.5 96.5L131.25 93.75V75H150V56.875H131.25V55.625A73.6 73.6 0 0 0 126.125 37.5L150 13.25L136.75 0L116.125 20.625A53.85 53.85 0 0 0 75 0A53.875 53.875 0 0 0 33.875 20.625L13.25 0L0 13.25L23.875 37.5A73.6125 73.6125 0 0 0 18.75 55.25V56.25H0V75H18.75V93.75L21.5 96.5L0 118L13.25 131.25L29.75 114.75A46.625 46.625 0 0 0 120.25 114.75zM94.8875 123.0125A28.125 28.125 0 0 1 46.875 103.125H103.125A28.125 28.125 0 0 1 94.8875 123.0125zM100.5 30.7625A41.625 41.625 0 0 1 112.5 56.25V84.375H37.5V56.25A41.625 41.625 0 0 1 75 18.75A41.625 41.625 0 0 1 100.5 30.75z")]
public class DebugOutNode : InputOutputNode
{
    private string _message;

    public DebugOutNode() : base("Debug Out")
    {
    }

    [Description("The text to display")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var msg = Evaluate<string>(Message);

        Debug.WriteLine(msg);

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
