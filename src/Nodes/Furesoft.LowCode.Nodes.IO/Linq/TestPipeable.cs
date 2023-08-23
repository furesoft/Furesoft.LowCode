using System.Collections;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[Description("Test")]
[NodeCategory("Linq")]
internal class TestPipeable : InputOutputNode, IPipeable
{
    public TestPipeable() : base("Testpipe")
    {
    }

    public IEnumerable PipeVariable { get; set; } = new List<string> {"hi", "bob"};

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken);
    }
}
