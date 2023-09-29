using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.Network.REST;

[Description("Send a REST Request To A Server")]
[NodeCategory("Network/REST")]
[NodeIcon(
    "M56 15V9C56 8.4688 55.5313 8 55 8H12V2C12 1.4688 11.5625 1 11 1 10.7188 1 10.4688 1.125 10.25 1.3125L.2813 11.3125C.0938 11.5 0 11.75 0 12 0 12.2813.0938 12.5313.2813 12.7188L10.2813 22.7188C10.4688 22.9063 10.75 23 11 23 11.5313 23 12 22.5625 12 22V16H55C55.5313 16 56 15.5625 56 15ZM56 32C56 31.75 55.9063 31.4688 55.7188 31.2813L45.7188 21.2813C45.5313 21.0938 45.25 21 45 21 44.4688 21 44 21.4688 44 22V28H1C.4688 28 0 28.4688 0 29V35C0 35.5313.4688 36 1 36H44V42C44 42.5625 44.4375 43 45 43 45.2813 43 45.5313 42.875 45.75 42.6875L55.7188 32.7188C55.9063 32.5313 56 32.25 56 32Z")]
public abstract class RestBaseNode : InputNode, IOutVariableProvider
{
    protected HttpClient client = new();

    protected RestBaseNode(string label) : base(label)
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public Evaluatable<string> URL { get; set; }

    [Pin("On Success", PinAlignment.Bottom)]
    public IOutputPin SuccessPin { get; set; }

    [Pin("On Failure", PinAlignment.Right)]
    public IOutputPin FailurePin { get; set; }

    public BindingList<string> Headers { get; set; } = new();

    [Required] public string OutVariable { get; set; }

    protected void CompileRequest(CodeWriter builder, HttpMethod method, object content = null)
    {
        CompileReadCall(builder, OutVariable, "Network.Rest.sendRequest", method, URL, Headers, content);
    }

    protected async Task ExecuteRequest(HttpMethod method, CancellationToken cancellationToken, object content = null)
    {
        var result = ScriptInitializer.SendRequest(method, URL, Headers, content);

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

}
