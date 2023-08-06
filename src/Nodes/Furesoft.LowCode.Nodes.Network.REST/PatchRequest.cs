using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PatchRequest : RestBaseNode, IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    public string Content { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public new string OutVariable { get; set; }

    public PatchRequest() : base("PATCH")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PatchAsync("/", new StringContent(Content), cancellationToken);
    }
}
