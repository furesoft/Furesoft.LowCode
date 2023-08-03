using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class GetRequest : RestBaseNode, IOutVariableProvider
{
    public string OutVariable { get; set; }

    public GetRequest() : base("GET")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.GetAsync("/", cancellationToken);
    }
}
