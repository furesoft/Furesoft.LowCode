using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Furesoft.LowCode.NodeViews;

namespace Furesoft.LowCode.Nodes.ControlFlow;

[NodeCategory("Control Flow")]
[NodeView(typeof(ConditionView))]
[Description("Change control flow based on condition")]
public class ConditionNode : InputNode
{

    public ConditionNode() : base("Condition")
    {
    }

    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    [Required]
    public Evaluatable Condition { get; set; }

    [Pin("True", PinAlignment.Bottom)] public IOutputPin TruePin { get; set; }

    [Pin("False", PinAlignment.Right)] public IOutputPin FalsePin { get; set; }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var value = Evaluate<bool>(Condition);

        if (value)
        {
            return ContinueWith(TruePin, cancellationToken: cancellationToken);
        }

        return ContinueWith(FalsePin, cancellationToken: cancellationToken);
    }
}
