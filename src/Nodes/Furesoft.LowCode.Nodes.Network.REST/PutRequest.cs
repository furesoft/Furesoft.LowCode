using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PutRequest : RestBaseNode, IOutVariableProvider
{
    public string Content { get; set; }
    public string OutVariable { get; set; }
    public PutRequest() : base("PUT")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PutAsync("/", new StringContent(Content), cancellationToken);
    }
}
