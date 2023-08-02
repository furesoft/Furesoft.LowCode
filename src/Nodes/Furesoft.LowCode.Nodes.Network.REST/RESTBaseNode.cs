using System.ComponentModel;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.Network.REST;

[IgnoreTemplate]
public abstract class RestBaseNode : InputOutputNode
{
    protected RestBaseNode(string label) : base(label)
    {
    }

    public string URL { get; set; }

    public BindingList<string> Header { get; set; } = new();
}
