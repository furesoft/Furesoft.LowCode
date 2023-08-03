using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class DeleteRequest : RestBaseNode, IOutVariableProvider
{
    public string Content { get; set; }
    public string OutVariable { get; set; }
    public DeleteRequest() : base("DELETE")
    {
    }

    public override async Task Invoke(CancellationToken cancellationToken, string evaluatedUrl)
    {
        var result = await client.DeleteAsync(evaluatedUrl, cancellationToken);
        
        SetOutVariable(OutVariable, result);
    }
}
