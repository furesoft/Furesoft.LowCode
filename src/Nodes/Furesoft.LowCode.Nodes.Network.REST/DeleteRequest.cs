﻿using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class DeleteRequest : RestBaseNode, IOutVariableProvider
{
    public string OutVariable { get; set; }

    public DeleteRequest() : base("DELETE")
    {
    }

    public override Task<HttpResponseMessage> Invoke(CancellationToken cancellationToken)
    {
        return client.DeleteAsync("/", cancellationToken);
    }
}
