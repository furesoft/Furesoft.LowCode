using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PutRequest : RestBaseNode, IOutVariableProvider
{
    public string Content { get; set; }
    public string OutVariable { get; set; }
    public PutRequest() : base("PUT")
    {
    }

    public override async Task Invoke(CancellationToken cancellationToken, string evaluatedUrl)
    {
        var result = await client.PutAsync(evaluatedUrl, new StringContent(Content), cancellationToken);
        
        SetOutVariable(OutVariable, result);
    }
}
