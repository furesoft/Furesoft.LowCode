namespace Furesoft.LowCode.Nodes;

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
