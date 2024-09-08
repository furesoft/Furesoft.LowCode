using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PatchRequest() : RestBaseNode("PATCH"), IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<object> Content { get; set; }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PatchAsync("/", new StringContent((string)Content), cancellationToken);
    }
}
