using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.Network.REST;

[Description("Send a REST Request To A Server")]
[NodeCategory("Network/REST")]
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
    public string OutVariable { get; set; }

    private void ApplyHeaders()
    {
        foreach (var header in Headers)
        {
            var spl = header
                .Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            client.DefaultRequestHeaders.Add(spl[0], spl[1]);
        }
    }

    public sealed override async Task Execute(CancellationToken cancellationToken)
    {
        ApplyHeaders();
        client.BaseAddress = new(URL);

        var response = await Invoke(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            SetOutVariable(OutVariable, await response.Content.ReadAsStringAsync(cancellationToken));
            await ContinueWith(SuccessPin, cancellationToken);
        }
        else
        {
            SetOutVariable(OutVariable, response.ReasonPhrase);
            await ContinueWith(FailurePin, cancellationToken);
        }
    }

    public abstract Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken);
}
