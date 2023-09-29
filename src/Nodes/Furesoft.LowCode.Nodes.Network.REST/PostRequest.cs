using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PostRequest : RestBaseNode, IOutVariableProvider
{
    public PostRequest() : base("POST")
    {
    }

    [DataMember(EmitDefaultValue = false)][Required] public Evaluatable<object> Content { get; set; }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await ExecuteRequest(HttpMethod.Post, cancellationToken, Content);
    }

    public override void Compile(CodeWriter builder)
    {
        CompileRequest(builder, HttpMethod.Post);
    }
}
