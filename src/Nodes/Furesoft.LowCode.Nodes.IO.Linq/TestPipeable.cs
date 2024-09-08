using System.ComponentModel;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[Description("Test")]
[NodeCategory("Linq")]
internal class TestPipeable() : InputOutputNode("Testpipe"), IPipeable
{
    public object PipeVariable { get; set; } = new List<string> {"hi", "bob"};

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken);
    }
}
