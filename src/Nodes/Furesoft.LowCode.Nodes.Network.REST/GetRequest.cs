using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class GetRequest : RestBaseNode, IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    public new string OutVariable { get; set; }

    public GetRequest() : base("GET")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.GetAsync("/", cancellationToken);
    }
}
