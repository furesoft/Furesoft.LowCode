using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PutRequest : RestBaseNode, IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    public string Content { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public string OutVariable { get; set; }
    public PutRequest() : base("PUT")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PutAsync("/", new StringContent(Content), cancellationToken);
    }
}
