using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PutRequest : RestBaseNode, IOutVariableProvider
{
    public PutRequest() : base("PUT")
    {
    }

    [DataMember(EmitDefaultValue = false)] 
    [Required]
    public string Content { get; set; }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PutAsync("/", new StringContent(Content), cancellationToken);
    }
}
