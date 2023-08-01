using Furesoft.LowCode.Designer.Core.Components.Views;

namespace Furesoft.LowCode.Nodes.Network;

[Description("Upload data to a website")]
[DataContract(IsReference = true)]
[NodeView(typeof(IconNodeView), "M194 131l-85 86 64 0-0 128 42 0 0-128 64 0zM45 3c-24-0-43 20-43 43l-0 256c-0 23 20 43 43 43l85 0 0-43-85-0 0-213 298 0-0 213-85-0-0 43 85 0c24 0 43-20 43-43l0-256c0-23-19-43-43-43l-298-0z")]
[NodeCategory("Network")]
public class UploadDataNode : VisualNode
{
    private string _url;
    private string _input;
    
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string URL
    {
        get => _url;
        set => SetProperty(ref _url, value);
    }
    
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Input
    {
        get => _input;
        set => SetProperty(ref _input, value);
    }

    public UploadDataNode() : base("Upload Data To Site")
    {
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var content = await client.GetStringAsync(Evaluate<string>(URL), cancellationToken);
        
        SetOutVariable(Input, content);

        await ContinueWith(OutputPin, cancellationToken: cancellationToken);
    }
}
