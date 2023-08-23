using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furesoft.LowCode.Attributes;

namespace Furesoft.LowCode.Nodes.IO.Linq;
[Description("Test")]
[NodeCategory("Linq")]
internal class TestPipeable : InputOutputNode, IPipeable
{
    public TestPipeable() :  base("Testpipe")
    {
    }

    private ICollection<string> PipeVariable { get; } = new List<string>()
    {
        "hi","bob"
    };

    public override Task Execute(CancellationToken cancellationToken)
    {
        return ContinueWith(OutputPin, cancellationToken);
    }

    public ICollection<object> GetPipe()
    {
        return PipeVariable.Cast<object>().ToList();
    }
}
