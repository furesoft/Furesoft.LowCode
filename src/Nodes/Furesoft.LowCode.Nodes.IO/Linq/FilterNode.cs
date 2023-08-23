using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
    [Description("Output Variable")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string OutVariable { get; set; }
    [Description("What is the filter for the query")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public Evaluatable<bool> Condition { get; set; }
    private ICollection<object> PipeVariable { get; set; }
    private IQueryable _queryable;
    public override Task Execute(CancellationToken cancellationToken)
    {
        var previous = GetPreviousNode<InputOutputNode>();
        if (previous is IPipeable pipe)
        {
            PipeVariable = pipe.GetPipe();
        }
        else if (previous is IOutVariableProvider outVariableProvider)
        {
            var pip = Evaluate(new Evaluatable<object>(outVariableProvider.OutVariable));
            if (pip is ICollection<object> pipes)
            {
                PipeVariable = pipes;
            }
        }

        List<object> result = new List<object>(PipeVariable.Count);
        foreach (var pi in PipeVariable)
        {
            DefineConstant("$", pi);
            if (Condition)
            {
                result.Add(pi);
            }
        }
        if (string.IsNullOrEmpty(OutVariable))
        {
            SetOutVariable(OutVariable, result);
        }
        PipeVariable = result;
        return ContinueWith(OutputPin, cancellationToken);
    }

    public ICollection<object> GetPipe() => PipeVariable;
}
