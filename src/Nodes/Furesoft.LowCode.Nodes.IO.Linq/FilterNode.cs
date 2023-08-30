using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[Description("Filters Nodes")]
[NodeIcon("M29.8667 64.3333l17.0667-10.6667v-17.0667l29.8667-36.2667h-76.8l29.8667 36.2667z")]
public class FilterNode : LinqNode
{
    public FilterNode() : base("Filter")
    {
    }

    [Description("What is the filter for the query")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public Evaluatable<bool> Condition { get; set; }

    protected override IEnumerable Iterate(IEnumerable src)
    {
        foreach (var pi in src)
        {
            DefineConstant("$", pi);

            if (Condition)
            {
                yield return pi;
            }
        }
    }
}
