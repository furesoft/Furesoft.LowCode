using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class PostRequest : RestBaseNode, IOutVariableProvider
{
    public string Content { get; set; }
    public string OutVariable { get; set; }
    public PostRequest() : base("POST")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.PostAsync("/", new StringContent(Content), cancellationToken);
    }
}
