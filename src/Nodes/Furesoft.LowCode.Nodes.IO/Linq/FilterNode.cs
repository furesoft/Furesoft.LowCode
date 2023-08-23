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
public class FilterNode : InputOutputNode,IOutVariableProvider,IPipeable<object>
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
    public ICollection<object> PipeVariable { get; private set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var previous = GetPreviousNode<InputOutputNode>();
        if (previous is IPipeable<object> pipe)
        {
            PipeVariable = pipe.PipeVariable;
        }
        else if (previous is IOutVariableProvider outVariableProvider)
        {
            var pip = new Evaluatable<object>(outVariableProvider.OutVariable);
            if (pip is ICollection<object> pipes)
            {
                PipeVariable = pipes;
            }
        }
        List<object> result = new List<object>(PipeVariable.Count);
        foreach (var pi in PipeVariable)
        {
            SetOutVariable("homp", pi);
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

}
