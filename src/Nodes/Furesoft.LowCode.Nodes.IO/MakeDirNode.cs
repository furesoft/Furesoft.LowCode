namespace Furesoft.LowCode.Nodes.IO;

[DataContract(IsReference = true)]
[Description("Create a directory")]
[NodeCategory("IO/Directories")]
public class MakeDirNode : VisualNode
{
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Path { get; set; }

    public MakeDirNode() : base("Make Directory")
    {
    }
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    
    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Evaluate<string>(Path));

        return ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
