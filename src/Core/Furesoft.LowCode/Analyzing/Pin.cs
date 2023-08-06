namespace Furesoft.LowCode.Analyzing;

public class Pin
{
    public bool HasConnection => Connections.Any();
    public IReadOnlyCollection<NodeReference> Connections { get; set; }
}
