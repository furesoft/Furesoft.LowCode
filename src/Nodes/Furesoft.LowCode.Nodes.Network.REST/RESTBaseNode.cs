using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

[Description("Send a POST Request To A Server")]
[DataContract(IsReference = true)]
[NodeCategory("Network/REST")]
public abstract class RestBaseNode : InputOutputNode
{
    protected RestBaseNode(string label) : base(label)
    {
    }

    public string URL { get; set; }

    public BindingList<string> Header { get; set; } = new();
}
