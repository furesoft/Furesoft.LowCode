using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PatchRequest : RestBaseNode, IOutVariableProvider
{
    public PatchRequest() : base("PATCH")
    {
    }

    [DataMember(EmitDefaultValue = false)] 
    [Required]
    public Evaluatable<object> Content { get; set; }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await ExecuteRequest(HttpMethod.Patch, cancellationToken, Content);
    }

    public override void Compile(CodeWriter builder)
    {
        CompileRequest(builder, HttpMethod.Patch);
    }
}
