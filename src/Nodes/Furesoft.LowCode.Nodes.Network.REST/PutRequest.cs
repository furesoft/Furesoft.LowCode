using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PutRequest : RestBaseNode, IOutVariableProvider
{
    public PutRequest() : base("PUT")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<object> Content { get; set; }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override void Compile(CodeWriter builder)
    {
        CompileRequest(builder, HttpMethod.Patch);
    }
}
