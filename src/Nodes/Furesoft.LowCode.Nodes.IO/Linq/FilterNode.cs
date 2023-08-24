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
    public IEnumerable PipeVariable { get; set; }
    [Description("Output Variable")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }
    public void ConvertPreviousToPipeable()
    {
        var node = GetPreviousNode<InputOutputNode>();
        if (node is IPipeable pipe)
        {
            PipeVariable = pipe.PipeVariable;
        }
        else if (node is IOutVariableProvider outVariableProvider)
        {
            var pip = Evaluate(new Evaluatable<object>(outVariableProvider.OutVariable));

            if (pip is IEnumerable pipes)
            {
                PipeVariable = pipes;
            }
            else
            {
                PipeVariable = new List<object>() { pip };
            }
        }
    }
    public override Task Execute(CancellationToken cancellationToken)
    {
        ConvertPreviousToPipeable();

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
        foreach (var pi in PipeVariable)
        {
            DefineConstant("$", pi);

            if (Condition)
            {
                yield return pi;
            }
        }
    }
}
