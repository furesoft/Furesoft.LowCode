using System.Collections;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Linq;

[Description("Filters Nodes")]
[NodeCategory("Linq")]
public class FilterNode : InputOutputNode, IOutVariableProvider, IPipeable
{
    private IQueryable _queryable;

    public FilterNode() : base("Filter")
    {
    }

    [Description("What is the filter for the query")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public Evaluatable<bool> Condition { get; set; }



    [Description("Output Variable")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var previous = GetPreviousNode<InputOutputNode>();

        if (previous is IPipeable pipe)
        {
            PipeVariable = pipe.PipeVariable;
        }
        else if (previous is IOutVariableProvider outVariableProvider)
        {
            var pip =  Evaluate(new Evaluatable<object>(outVariableProvider.OutVariable));

            if (pip is IEnumerable pipes)
            {
                PipeVariable = pipes;
            }
        }

        var result = new List<object>();

        foreach (var pi in PipeVariable)
        {
            DefineConstant("$", pi);

            if (Condition)
            {
                result.Add(pi);
            }
        }

        if (!string.IsNullOrEmpty(OutVariable))
        {
            SetOutVariable(OutVariable, result);
        }

        PipeVariable = result;

        return ContinueWith(OutputPin, cancellationToken);
    }

    public IEnumerable PipeVariable { get; set; }
}
