namespace Furesoft.LowCode.Analyzing;

public class NodeReference
{
    public List<Pin> Inputs { get; set; }
    public List<Pin> Outputs { get; set; }
    public object Node { get; set; }
}
