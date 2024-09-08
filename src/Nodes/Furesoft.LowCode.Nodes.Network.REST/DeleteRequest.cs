using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class DeleteRequest() : RestBaseNode("DELETE"), IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.DeleteAsync("/", cancellationToken);
    }
}
