using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.IO.Linq;
[Description("Test")]
[NodeCategory("Linq")]
internal class TestPipeable : InputOutputNode, IPipeable<object>
{
    public TestPipeable() :  base("Testpipe")
    {
    }

    public ICollection<object> PipeVariable { get; } = new List<object>()
    {
        "hi","bob"
    };

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken);
    }
}
