using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class DeleteRequest : RestBaseNode, IOutVariableProvider
{
    public DeleteRequest() : base("DELETE")
    {
    }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override void Compile(CodeWriter builder)
    {
        CompileRequest(builder, HttpMethod.Delete);
    }
}
