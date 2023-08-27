using System.Collections;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[Description("Filters Nodes")]
[NodeCategory("Linq")]
public class FilterNode : InputOutputNode, IOutVariableProvider, IPipeable
{
    public FilterNode() : base("Filter")
    {
    }

    [Description("What is the filter for the query")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public Evaluatable<bool> Condition { get; set; }

    [Description("Output Variable")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }

    public object PipeVariable { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {


        var result = LazyIteratePipeVariable();

        if (!string.IsNullOrEmpty(OutVariable))
        {
            SetOutVariable(OutVariable, result);
        }

        PipeVariable = result;

        return ContinueWith(OutputPin, cancellationToken);
    }

    public IEnumerable LazyIteratePipeVariable()
    {
        if (PipeVariable is IEnumerable p)
        {
            foreach (var pi in p)
            {
                DefineConstant("$", pi);

                if (Condition)
                {
                    yield return pi;
                }
            }
        }
    }
}
