using System.ComponentModel;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

[Description("Send a POST Request To A Server")]
[NodeCategory("Network/REST")]
public abstract class RestBaseNode : InputOutputNode
{
    protected RestBaseNode(string label) : base(label)
    {
    }

    public string URL { get; set; }

    public BindingList<string> Headers { get; set; } = new();

    protected HttpClient client = new();

    private void ApplyHeaders()
    {
        foreach (var header in Headers)
        {
            var spl = Evaluate<string>(header).Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            client.DefaultRequestHeaders.Add(spl[0], spl[1]);
        }
    }

    public sealed override Task Execute(CancellationToken cancellationToken)
    {
        ApplyHeaders();

        return Invoke(cancellationToken, Evaluate<string>(URL));
    }

    public abstract Task Invoke(CancellationToken cancellationToken, string evaluatedURL);
}
