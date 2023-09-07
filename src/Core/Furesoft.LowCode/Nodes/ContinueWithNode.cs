namespace Furesoft.LowCode.Nodes;

[NodeIcon(
    "M10 9A5 5 0 015 14 5 5 0 010 9C0 6.58 1.72 4.56 4 4.1V0H6V4.1C8.28 4.56 10 6.58 10 9M5 6A3 3 0 002 9 3 3 0 005 12 3 3 0 008 9 3 3 0 005 6M4 18V16H6V18H4Z")]
public class ContinueWithNode : InputOutputNode
{
    public ContinueWithNode(string label) : base(label)
    {
    }

    public string PinName { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(PinName, cancellationToken);
    }
}
