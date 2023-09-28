using System.Runtime.Serialization;
using Furesoft.LowCode.Compilation;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class GetRequest : RestBaseNode, IOutVariableProvider
{
    public GetRequest() : base("GET")
    {
    }

    [DataMember(EmitDefaultValue = false)] public new string OutVariable { get; set; }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        var result = ScriptInitializer.SendRequest(HttpMethod.Get, URL, Headers);

        if (result.IsSuccess)
        {
            SetOutVariable(OutVariable, result.Value);
            await ContinueWith(SuccessPin, cancellationToken);
        }
        else
        {
            SetOutVariable(OutVariable, result.Value);
            await ContinueWith(FailurePin, cancellationToken);
        }
    }

    public override void Compile(CodeWriter builder)
    {
        CompileReadCall(builder, OutVariable, "Network.Rest.sendRequest", HttpMethod.Get, URL, Headers);
    }
}
