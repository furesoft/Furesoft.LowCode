using System.ComponentModel;

namespace Furesoft.LowCode.Nodes.Network;

[Description("Gets the content of a website")]
[DataContract(IsReference = true)]
[NodeCategory]
public class GetSiteContentNode : VisualNode
{
    private string _url;
    private string _outputVariable;

    [Pin("Input Flow", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }
    
    [Pin("Output Flow", PinAlignment.Top)]
    public IOutputPin OutPin { get; set; }

    public string URL
    {
        get => _url;
        set => SetProperty(ref _url, value);
    }

    public string OutputVariable
    {
        get => _outputVariable;
        set => SetProperty(ref _outputVariable, value);
    }

    public GetSiteContentNode() : base("Get Site Content")
    {
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var client = new HttpClient();
        var content = await client.GetStringAsync(URL, cancellationToken);
        
        SetOutVariable(OutputVariable, content);
    }
}
