using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PostRequest : RestBaseNode, IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    public string Content { get; set; }
    
    [DataMember(EmitDefaultValue = false)]
    public new string OutVariable { get; set; }
    public PostRequest() : base("POST")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PostAsync("/", new StringContent(Content), cancellationToken);
    }
}
